using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB_CSharp
{
    class Program
    {
        /// <summary>
        /// É mais seguro né! 
        /// 1. Você precisa criar um usuário no seu mongodb
        /// 2. garantir o acesso a todos os databases
        /// 3. Criar um novo usuário em seu database e conceder o acesso que você quiser
        /// </summary>
        public static void CriandoConexaoAutenticada()
        {
            try
            {
                var settings = new MongoClientSettings
                {
                    ServerSelectionTimeout = new TimeSpan(0, 0, 5),
                    Server = new MongoServerAddress("localhost", 27017),
                    Credentials = new[]
                    {
                        MongoCredential.CreateCredential("loja", "felipe", "felipe123456")
                    }
                };

                var client = new MongoClient(settings);
                var database = client.GetDatabase("loja");
                var colecao = database.GetCollection<Usuario>("usuarios");

                //se tiver registro no mongdb sem endereço, retornara um erro porque não tratamos no override do tostring
                var filtro = Builders<Usuario>.Filter.Empty;
                var usuarios = colecao.Find(filtro).ToList();

                usuarios.ForEach(u => Console.WriteLine(u));

            }
            catch (Exception ex)
            {
                Console.Write($"Erro: {ex.Message}");
            }
        }


        static void Main(string[] args)
        {
            try
            {

                CriandoConexaoAutenticada();
                //var client = new MongoClient("mongodb://localhost:27017");
                //var database = client.GetDatabase("loja");
                //var colecao = database.GetCollection<Usuario>("usuarios");


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            Console.ReadLine();
        }

        public static void InserirUmUsuario(IMongoCollection<Usuario> colecao)
        {
            //Inserindo apenas um usuário:
            var usuario = new Usuario()
            {
                Login = "felipe",
                Senha = "123456"
            };
            colecao.InsertOne(usuario);
        }

        public static void InserirMuitosUsuarios(IMongoCollection<Usuario> colecao)
        {
            //Inserindo mais de um usuário
            var usuarios = new List<Usuario>()
                {
                    new Usuario{
                    Login = "bruno",
                    Senha = "123457"
                    },
                    new Usuario{
                    Login = "julia",
                    Senha = "123458"
                    },
                };
            colecao.InsertMany(usuarios);
        }

        /// <summary>
        /// No MongoDB você tem funções para executar em apenas uma linha ou muitas linhas, em caso de ter mais de uma linha e você executar uma função de uma única linha ele vai executar na primeira linha encontrada.
        /// </summary>
        /// <param name="colecao"></param>
        public static void AtualizandoUmUsuario(IMongoCollection<Usuario> colecao)
        {
            //Atualizando o usuario
            var filter = Builders<Usuario>.Filter.Eq(u => u.Login, "felipe");
            var update = Builders<Usuario>.Update.Set(u => u.Senha, "abc123456");
            colecao.UpdateOne(filter, update);
        }

        public static void AtualizandoMuitosUsuarios(IMongoCollection<Usuario> colecao)
        {
            //Atualizando mais de um usuário:
            var filter = Builders<Usuario>.Filter.Empty; //um select sem where
            var update = Builders<Usuario>.Update.Set(u => u.Ativo, false);
            colecao.UpdateMany(filter, update);
            Console.WriteLine("Usuário inserido com sucesso!");
        }

        public static void DeleteUmUsuario(IMongoCollection<Usuario> colecao)
        {
            //Deletando um usuário:
            var filter = Builders<Usuario>.Filter.Eq(u => u.Login, "samuel");
            var resultado = colecao.DeleteOne(filter);
            Console.WriteLine($"{resultado.DeletedCount} documento(s) excluído(s).");
        }

        public static void DeleteMuitosUsuarios(IMongoCollection<Usuario> colecao)
        {
            //Deletando vários usuários:
            var filter = Builders<Usuario>.Filter.Where(u => u.Login == "bruno" || u.Login == "julia");
            var resultado = colecao.DeleteMany(filter);
            Console.WriteLine($"{resultado.DeletedCount} documento(s) excluído(s).");
        }

        public static void consultarUsuarios(IMongoCollection<Usuario> colecao)
        {
            //Consultando os dados:
            var filter = Builders<Usuario>.Filter.Empty; //um select sem where
            var resultados = colecao.Find(filter).ToList();

            resultados.ForEach(u => Console.WriteLine(u));
        }


        /// <summary>
        /// Insert simples, mas dessa vez com uma classe e uma subclasse.
        /// </summary>
        /// <param name="colecao"></param>
        public static void InserirUmUsuarioEEndereco(IMongoCollection<Usuario> colecao)
        {
            var usuarios = new List<Usuario>()
                {
                    new Usuario{
                    Login = "bruno",
                    Senha = "123457",
                    Endereco = new Endereco
                    {
                        Logradouro = "Rua armanado vieira",
                        Numero = 177,
                        Bairro = "Barra da Tijuca",
                        Cidade = "Rio de Janeiro",
                        UF = "RJ"
                    }
                    },
                    new Usuario{
                    Login = "julia",
                    Senha = "123458",
                     Endereco = new Endereco
                    {
                        Logradouro = "Rua armanado vieira",
                        Numero = 177,
                        Bairro = "Barra da Tijuca",
                        Cidade = "Rio de Janeiro",
                        UF = "RJ"
                    }
                    },
                };

            colecao.InsertMany(usuarios);
        }

        /// <summary>
        /// Nova consulta, mas dessa vez utilizando a subpropriedade
        /// </summary>
        /// <param name="colecao"></param>
        public static void ConsultarUsuariosEEndereco(IMongoCollection<Usuario> colecao)
        {
            var filtro = Builders<Usuario>.Filter.Eq(u => u.Endereco.UF, "RJ");
            var usuarios = colecao.Find(filtro).ToList();

            usuarios.ForEach(u => Console.WriteLine(u));
        }

        /// <summary>
        /// Por default o MongoDB não verifica a conexão, então para capturar uma exceção de TimeOut precisamos prever isso em um try catch
        /// </summary>
        public static void CriandoUmTimeoutNoMondoDB()
        {
            try
            {
                var settings = new MongoClientSettings
                {
                    ServerSelectionTimeout = new TimeSpan(0, 0, 5),
                    Server = new MongoServerAddress("localhost", 27017)
                };

                var client = new MongoClient(settings);
                var database = client.GetDatabase("loja");
                var colecao = database.GetCollection<Usuario>("usuarios");

                var filtro = Builders<Usuario>.Filter.Eq(u => u.Endereco.UF, "RJ");
                var usuarios = colecao.Find(filtro).ToList();

                usuarios.ForEach(u => Console.WriteLine(u));

            }
            catch (TimeoutException oe)
            {
                Console.Write($"Erro: Não foi possível se conectar com o servidor. Message: {oe.Message}");
            }
        }
    }
}
