using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DeepCore.IO;

namespace Cell.IO
{
    public class ImageIO
    {

        /*
        2.数据块结构

        PNG文件中，除了PNG文件标识外，所有的数据块均由４个部分组成，如下表：

        ----------------------------------------------------------------------
        顺号 名称　　    字节数　　说明  
        ----------------------------------------------------------------------
        １　长度　　　　　4  　　 指定第３部分数据域的长度  
        ２　数据块符号　　4  　　 由数据块符号的 Ascii 码组成  
        ３　数据域　　   不定     存储按照 Chunk Type Code 指定的数据  
        ４　CRC校验   　 4　　　　又称循环冗余检测，用来检测是否有错误
        ----------------------------------------------------------------------
        循环冗余检测中的值是对第２部分数据块符号和第３部分数据域进行计算得到的，
        具体算法定义在ISO 3309 和　ITU-T V.42中，其值按下面的 CRC 码生成多项式进行计算：
        x32+x26+x23+x22+x16+x12+x11+x10+x8+x7+x5+x4+x2+x+1
        */
        static byte[] PNG_HEAD =
            new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
        static byte[] PNG_END =
            new byte[] { 0x49, 0x45, 0x4E, 0x44, 0xAE, 0x42, 0x60, 0x82 };

        public static bool IsPngHead(byte[] data)
        {
            byte[] head = new byte[8];
            Array.Copy(data, 0, head, 0, 8);
            if (head.SequenceEqual(PNG_HEAD))
            {
                return true;
            }
            return false;
        }
        
        public static MemoryStream decodePNGStream(Stream images_fs)
        {
            long beginPos = images_fs.Position;
            byte[] head = new byte[8];
            images_fs.Read(head, 0, 8);
            if (head.SequenceEqual(PNG_HEAD))
            {
                do
                {
                    uint len = LittleEdian.readU32(images_fs);
                    if (len == 0)
                    {
                        byte[] tail = new byte[8];
                        images_fs.Read(tail, 0, 8);
                        if (tail.SequenceEqual(PNG_END))
                        {
                            break;
                        }
                    }
                    else
                    {
                        images_fs.Position += (4 + len + 4);
                    }
                }
                while (true);

                long endPos = images_fs.Position;
                int pngLen = (int)(endPos - beginPos);

                MemoryStream ms = IOUtil.copy(images_fs, beginPos, pngLen);
                ms.Position = 0;
                return ms;
            }
            return null;
        }
        

        private static byte[] NEW_HEADER = new byte[] { (byte)0, (byte)1, (byte)2, (byte)3, };

        public static bool IsNewHeader(Stream stream)
        {
            long oldp = stream.Position;
            try
            {
                byte[] head = DeepCore.IO.IOUtil.ReadExpect(stream, 4);
                if (head.SequenceEqual(NEW_HEADER))
                {
                    return true;
                }
            }
            finally
            {
                stream.Position = oldp;
            }
            return false;
        }

        public static bool DecodeNewFromStream(Stream stream, Action<int, byte[], Exception> decode)
        {
            var input = new InputStream(stream, null);
            byte[] head = DeepCore.IO.IOUtil.ReadExpect(stream, 4);
            if (head.SequenceEqual(NEW_HEADER))
            {
                int count = input.GetS32();
                for (int i = 0; i < count; i++)
                {
                    int length = input.GetS32();
                    try
                    {
                        if (length >= 0)
                        {
                            byte[] raw = new byte[length];
                            input.GetRawData(raw, 0, length);
                            decode(i, raw, null);
                        }
                        else
                        {
                            decode(i, null, null);
                        }
                    }
                    catch (Exception err)
                    {
                        decode(i, null, err);
                    }
                }
                return true;
            }
            return false;
        }
        public static void EncodeNewToStream(Stream stream, int count, Func<int, byte[]> encode)
        {
            var output = new OutputStream(stream, null);
            output.PutRawData(NEW_HEADER, 0, 4);
            output.PutS32(count);
            for (int i = 0; i < count; i++)
            {
                try
                {
                    var raw = encode(i);
                    if (raw != null)
                    {
                        output.PutS32(raw.Length);
                        output.PutRawData(raw, 0, raw.Length);
                    }
                    else
                    {
                        output.PutS32(-1);
                    }
                }
                catch
                {
                    output.PutS32(-1);
                }
            }
        }
    }
}
