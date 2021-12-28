using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathModule
{
    public class MathObject
    {
        /// <summary>
        /// Переменные
        /// 
        /// id - идентификатор объекта
        /// status - статус объекта
        /// </summary>
        public string id = "0";
        private int status = 0;


        /// <summary>
        /// Просто ToStrung()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return id;
        }


        /// <summary>
        /// Конструктор без элементов
        /// </summary>
        public MathObject()
        {
        }



        /// <summary>
        /// Конструктор для установки id и status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        public MathObject(string id, int status = 0)
        {
            this.id = id;
            this.status = status;
        }



        /// <summary>
        /// Конструктор для установки status
        /// </summary>
        /// <param name="status"></param>
        public MathObject(int status)
        {
            this.status = status;
        }



        /// <summary>
        /// Возвращяет статус объекта
        /// </summary>
        /// <returns></returns>
        public int GetStatus()
        {
            return status;
        }



        /// <summary>
        /// Устанавливает статус объекта
        /// </summary>
        /// <param name="status"></param>
        public void SetStatus(int status)
        {
            this.status = status;
        }
    }
}
