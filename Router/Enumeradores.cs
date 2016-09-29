using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Router
{
    class Enumeradores
    {
        public enum PrioridadCola {Alta = 5, Media = 3, Baja = 1};
        public enum EstadoSistema { Desocupado = 0, Ocupado = 1};
        public enum ProximoEvento { Arribo = 0, Partida = 1};
        public enum modo { FIFO = 0, Prioridades = 1};
    }
}
