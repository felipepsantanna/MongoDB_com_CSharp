using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MongoDB_CSharp
{
    public class Usuario
    {
        public ObjectId _id { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public bool Ativo { get; set; }
        public Endereco Endereco { get; set; }

        public override string ToString()
        {
            return $"{Login}{System.Environment.NewLine}{Endereco.Logradouro}, {Endereco.Numero}.{Endereco.Bairro    }.{Endereco.Cidade}-{Endereco.UF}{System.Environment.NewLine}";
        }
    }
}
