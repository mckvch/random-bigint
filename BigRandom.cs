using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigInt = System.Numerics.BigInteger;

namespace SmallShitLibrary.Gens
{
    /// <summary>
    /// Генератор огромных чисел.
    /// </summary>
    public class BigRandom
    {
        /// <summary>
        /// Перечисление, содержащее параметры чётности для генератора.
        /// </summary>
        public enum EvenMode
        {
            /// <summary>
            /// 
            /// </summary>
            Random = 0,
            /// <summary>
            /// 
            /// </summary>
            NotEven = 1,
            /// <summary>
            /// 
            /// </summary>
            Even = 2
        }

        private Random kr;

        private byte[] Gen_Bytes(int bytePower)
        {
            //byte[] number = { 255, 255, 255, 255, 0 };//первый байт - младший(типа последний справа), последний байт - старший
            //byte[] number = new byte[385];//самый старший байт - мусор для минуса
            byte[] returnBytes = new byte[bytePower + 1];
            kr.NextBytes(returnBytes);
            returnBytes[bytePower] = 0;//старший байт под минус
            returnBytes[bytePower - 1] = (byte)kr.Next(128, 256);//второй по старшинству байт генерируется с единицей в самом старшем бите для обеспечения заданной битности числа
            return returnBytes;
        }

        private BigInt GetBigInt(byte[] MaxValue)
        {
            int L = MaxValue.Length - 1;
            byte[] wGet = new byte[MaxValue.Length];

            if (MaxValue[L] == 0)
            {
                wGet[L] = 0;
                L -= 1;
            }

            wGet[L] = (byte)kr.Next(0, MaxValue[L] + 1);//1
            while (wGet[L] == MaxValue[L])          //1.1
            {
                L -= 1;
                wGet[L] = (byte)kr.Next(0, MaxValue[L] + 1);
            }

            while (L > 0)                 //1.2
            {
                L -= 1;
                wGet[L] = (byte)kr.Next(0, 256);
            }

            return new BigInt(wGet);
        }

        //////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Конструктор BigRandom.
        /// </summary>
        public BigRandom()
        {
            this.kr = new Random();
        }

        /// <summary>
        /// 
        /// </summary>
        public void NewRandomSeed()
        {
            kr = new Random();
        }

        /// <summary>
        /// Возвращает случайное число, не превышающее MaxValue.
        /// </summary>
        /// <param name="MaxValue">Верхний исключённый предел.</param>
        /// <returns>Случайное число, не превышающее MaxValue.</returns>
        public BigInt Next_BigInt(BigInt MaxValue)
        {
            return GetBigInt(MaxValue.ToByteArray());
        }

        /// <summary>
        /// Возвращает случайное число, не превышающее MaxValue.
        /// </summary>
        /// <param name="MaxValue">Верхний исключённый предел. (Байтовое представление числа.)</param>
        /// <returns>Случайное число, не превышающее MaxValue.</returns>
        public BigInt Next_BigInt(byte[] MaxValue)
        {
            return GetBigInt(MaxValue);
        }

        /// <summary>
        /// Возвращает число заданной разрядности и чётности.
        /// </summary>
        /// <param name="bytePower">Разрядность числа в байтах(!).</param>
        /// <param name="Mode">Чётность числа.</param>
        /// <returns>Случайное число заданных разрядности (bytePower) и чётности (Mode).</returns>
        public BigInt Next_BigInt(int bytePower, EvenMode Mode)
        {
            byte[] returnBytes = Gen_Bytes(bytePower);
            switch (Mode)
            {
                case EvenMode.Even:
                    if (returnBytes[0] % 2 == 1)//здесь число делается чётным
                        returnBytes[0] -= 1;
                    return new BigInt(returnBytes);

                case EvenMode.NotEven:
                    if (returnBytes[0] % 2 == 0)//здесь число делается нечётным
                        returnBytes[0] += 1;
                    return new BigInt(returnBytes);

                case EvenMode.Random:
                    return new BigInt(returnBytes);

                default:
                    throw new Exception("Неверный параметр EvenMode.");
            }

        }

    }
}