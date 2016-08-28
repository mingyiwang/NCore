using System;

namespace Core.Primitive
{
    public enum ByteOrder
    {
        /// <summary>
        /// Constant denoting little-endian byte order.  In this order, the bytes of
        /// a multibyte value are ordered from least significant to most
        /// significant.
        /// </summary>
        LittleEndian,

        /// <summary>
        /// Constant denoting big-endian byte order.  In this order, the bytes of a
        /// multibyte value are ordered from most significant to least significant.
        /// </summary>
        BigEndian

    }

    public static class ByteOrderExtension
    {
        public static ByteOrder GetOrder(this ByteOrder order)
        {
            return BitConverter.IsLittleEndian 
                 ? ByteOrder.LittleEndian 
                 : ByteOrder.BigEndian
                 ;
        }
    }

}