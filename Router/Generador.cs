using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Router
{
    class Generador
    {
        private Random random;
        public Generador()
        {
            random = new Random();
        }
        public double generaArribo(double lambda)
        {
            //TODO: programar, copiar del tp anterior
            return null;
        }
        public double generaServicio(double mu)
        {
            //TODO: programar, copiar del tp anterior
            return null;
        }
        public double generaNumeroAleatorio()   //Se usa para determinar la prioridad del paquete
        {
            return random.NextDouble();
        }
    }
}
