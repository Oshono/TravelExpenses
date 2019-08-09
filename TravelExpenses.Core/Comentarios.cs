using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Core
{
    public class Comentarios
    {
        public int IdComentarios { get; set; }
        public string Comentario { get; set; }
        public int Folio { get; set; }

        public static implicit operator Comentarios(List<Comentarios> v)
        {
            throw new NotImplementedException();
        }
    }
}
