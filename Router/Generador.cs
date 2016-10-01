using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Router
{
    class Generador
    {
        private static Random random = new Random();
        public static double generaArribo(double lambda)
        {
            double U = random.NextDouble();
            return (-1 * Math.Log(U)) / lambda;
        }
        public static double generaServicio(double mu)
        {
            double U = random.NextDouble();
            return (-1 * Math.Log(U)) / mu;
        }
        public static double generaNumeroAleatorio()   //Se usa para determinar la prioridad del paquete
        {
            return random.NextDouble();
        }      
    }
}
