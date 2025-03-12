using Microsoft.AspNetCore.Mvc;
#if ADICOMSOFT
using ObstructionGameLib;
#endif
using System.Reflection;

namespace Backend_Obstruction.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class JocController : ControllerBase
    {
#if !ADICOMSOFT

   //Am modificat algoritmul si am incercat sa il fac mai eficient.
        
        [HttpGet("AlgoritmJoc"), AlgoritmJoc("Implementat de utilizator")]
        public Task<string> AlgoritmJoc(string formula)
        {
            // Este necesar sa furnizati implementarea acestei metode, cu respectarea indicatiilor din documentatie.
            // Retineti va rugam:
            // - Algoritmul implementat va fi comparat cu algoritmul "Random" din implementarea Cloud. Algoritmul implementat
            //   trebuie de catre dumneavoastra trebuie sa fie mai bun decat algoritmul "Random" din implementarea Cloud pe toate
            //   dimensiunile de tabla de joc. Acesta este un criteriu minimal - pentru maximum de puncte algoritmul implementat
            //   de catre dumneavoastra trebuie sa fie mai bun decat algoritmul "Easy" din implementarea cloud.
            // - Se evalueza implementarea algoritmului, nu doar rezultatul. Ne dorim sa vedem structuri de date eficiente, algoritmi
            //   implementati eficient si cod bine organizat.
            
            {

              
                {
                    // Inițializarea variabilelor și a tablei de joc
                    int rows = formula[0] - '0';
                    int cols = formula[1] - '0';
                    char[,] board = new char[rows, cols];
                    int[] dRows = { -1, 0, 1, 0, -1, -1, 1, 1 };
                    int[] dCols = { 0, 1, 0, -1, -1, 1, -1, 1 };
                    int numX = 0, numO = 0;

                    if (string.IsNullOrEmpty(formula) || formula.Length < 3)
                        return Task.FromResult("EROARE_TABLA");

                    string boardData = formula.Substring(2);
                    if (boardData.Length != rows * cols || !boardData.All(c => "XxOo0_-".Contains(c)))
                        return Task.FromResult("EROARE_TABLA");

                    // Inițializarea tablei și numărarea simbolurilor
                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < cols; j++)
                        {
                            char c = boardData[i * cols + j];
                            board[i, j] = c;
                            if (c == 'X' || c == 'x') numX++;
                            if (c == 'O' || c == 'o' || c == '0') numO++;
                        }
                    }

                    // Blocarea celulelor adiacente simbolurilor existente
                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < cols; j++)
                        {
                            if (board[i, j] == 'X' || board[i, j] == 'O')
                            {
                                for (int d = 0; d < 8; d++)
                                {
                                    int newRow = i + dRows[d];
                                    int newCol = j + dCols[d];

                                    if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols && board[newRow, newCol] == '_')
                                    {
                                        board[newRow, newCol] = '-'; // Blocăm celulele
                                    }
                                }
                            }
                        }
                    }

                    if (Math.Abs(numX - numO) > 1)
                        return Task.FromResult("EROARE_TABLA");

                    char currentPlayer = numX > numO ? 'O' : 'X';

                    // Căutarea celei mai bune mutări disponibile
                    int bestRow = -1, bestCol = -1;
                    int bestScore = int.MinValue;

                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < cols; j++)
                        {
                            if (board[i, j] == '_')
                            {
                                // Facem o mutare virtuală
                                board[i, j] = currentPlayer;

                                // Blocăm celulele adiacente pentru simbolul mutat
                                for (int d = 0; d < 8; d++)
                                {
                                    int newRow = i + dRows[d];
                                    int newCol = j + dCols[d];

                                    if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols && board[newRow, newCol] == '_')
                                    {
                                        board[newRow, newCol] = '-';
                                    }
                                }

                                // Calculăm scorul acestei mutări
                                int currentScore = CalculateScore(board, rows, cols, currentPlayer);

                                // Revenim la starea inițială
                                board[i, j] = '_';
                                for (int d = 0; d < 8; d++)
                                {
                                    int newRow = i + dRows[d];
                                    int newCol = j + dCols[d];
                                    if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols && board[newRow, newCol] == '-')
                                    {
                                        board[newRow, newCol] = '_';
                                    }
                                }

                                // Alegem mutarea cu cel mai bun scor
                                if (currentScore > bestScore)
                                {
                                    bestScore = currentScore;
                                    bestRow = i;
                                    bestCol = j;
                                }
                            }
                        }
                    }

                    if (bestRow != -1 && bestCol != -1)
                    {
                        board[bestRow, bestCol] = currentPlayer;

                        // Blocăm celulele adiacente pentru simbolul plasat
                        for (int d = 0; d < 8; d++)
                        {
                            int newRow = bestRow + dRows[d];
                            int newCol = bestCol + dCols[d];

                            if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols && board[newRow, newCol] == '_')
                            {
                                board[newRow, newCol] = '-';
                            }
                        }

                        // Verificăm dacă mai există mutări disponibile
                        bool hasMoves = false;
                        for (int r = 0; r < rows; r++)
                        {
                            for (int c = 0; c < cols; c++)
                            {
                                if (board[r, c] == '_')
                                {
                                    hasMoves = true;
                                    break;
                                }
                            }
                            if (hasMoves) break;
                        }

                        return Task.FromResult(hasMoves ? $"{bestRow}{bestCol}" : $"{bestRow}{bestCol}:FINAL");
                    }

                    return Task.FromResult("NU_EXISTA");
                }

            }
            throw new NotImplementedException();
        }

        private static int CalculateScore(char[,] board, int rows, int cols, char player)
        {
            int score = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (board[i, j] == player)
                    {
                        score++;
                    }
                }
            }
            return score;
        }

        // Am impartit codul in functii private in Implementarea Alternativa, incercand sa evit codul de tip -spaghetti-

        [HttpGet("AlgoritmJoc2"), AlgoritmJoc("Implementare alternativa")]
        public Task<string> AlgoritmJoc2(string formula)
        {
            // Inițializam variabilele și a tabla de joc
            int rows = formula[0] - '0';
            int cols = formula[1] - '0';
            char[,] board = new char[rows, cols];
            int[] dRows = { -1, 0, 1, 0, -1, -1, 1, 1 };
            int[] dCols = { 0, 1, 0, -1, -1, 1, -1, 1 };
            int numX = 0, numO = 0;
                        
            if (string.IsNullOrEmpty(formula) || formula.Length < 3)
                return Task.FromResult("EROARE_TABLA");

            string boardData = formula.Substring(2);
            if (boardData.Length != rows * cols || !boardData.All(c => "XxOo0_-".Contains(c)))
                return Task.FromResult("EROARE_TABLA");

            // Inițializam tabla de joc și numaram simbolurile
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    char c = boardData[i * cols + j];
                    board[i, j] = c;
                    if (c == 'X' || c == 'x') numX++;
                    if (c == 'O' || c == 'o' || c == '0') numO++;
                }
            }

            // Blocharea celulelor adiacente 
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (board[i, j] == 'X' || board[i, j] == 'O')
                    {
                        for (int d = 0; d < 8; d++)
                        {
                            int newRow = i + dRows[d];
                            int newCol = j + dCols[d];

                            if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols)
                            {
                                if (board[newRow, newCol] == 'X' || board[newRow, newCol] == 'O')
                                {
                                    return Task.FromResult("EROARE_TABLA");
                                }
                                else if (board[newRow, newCol] == '_')
                                {
                                    board[newRow, newCol] = '-';
                                }
                            }
                        }
                    }
                }
            }
                        
            if (Math.Abs(numX - numO) > 1)
                return Task.FromResult("EROARE_TABLA");

            char currentPlayer = numX > numO ? 'O' : 'X';

            // Căutarea unui loc pentru simbol 
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (board[i, j] == '_')
                    {
                        board[i, j] = currentPlayer;

                        // Blochează celulele adiacente pentru simbol
                        for (int d = 0; d < 8; d++)
                        {
                            int newRow = i + dRows[d];
                            int newCol = j + dCols[d];

                            if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols && board[newRow, newCol] == '_')
                            {
                                board[newRow, newCol] = '-';
                            }
                        }

                        // Verificam dacă mai există mutări disponibile
                        bool hasMoves = false;
                        for (int r = 0; r < rows; r++)
                        {
                            for (int c = 0; c < cols; c++)
                            {
                                if (board[r, c] == '_')
                                {
                                    hasMoves = true;
                                    break;
                                }
                            }
                            if (hasMoves) break;
                        }

                        //Daca nu mai sunt mutari disponibile, returnam mesaj
                        return Task.FromResult(hasMoves ? $"{i}{j}" : $"{i}{j}:FINAL");
                    }
                }
            }

            return Task.FromResult("NU_EXISTA");
        }

#endif

#if ADICOMSOFT
        // Acest fisier de cod sursa face parte din implementarea AdiComSoft a jocului Obstruction. Observati cum accesul la implementarea
        // ACS este limitata prin utilizarea directivelor de precompilare.

        [HttpGet("MutareRandom"), AlgoritmJoc("Random")]
        public Task<string> MutareRandom(string formula)
        {
            return (new VariantaAlgoritm_Random(0)).Mutare(formula);
        }

        [HttpGet("MutareRandomCuGreseli"), AlgoritmJoc("Returneaza intentionat mutari gresite")]
        public Task<string> MutareRandomCuGreseli(string formula)
        {
            return (new VariantaAlgoritm_Random(33)).Mutare(formula);
        }

        [HttpGet("MutareEasy"), AlgoritmJoc("Easy - ceva simplu")]
        public Task<string> MutareEasy(string formula)
        {
            return (new VariantaAlgoritm_Easy()).Mutare(formula);
        }

        [HttpGet("Mutare"), AlgoritmJoc("Complet? Complet sa fie.")]
        public Task<string> Mutare(string formula)
        {
            return (new VariantaAlgoritm_Random(33)).Mutare(formula);
        }
#endif

        /// <summary>
        /// Aceasta metoda identifica toate implementarile algoritmului de joc disponibile in solutia curenta. Metoda este utilizata de UI pentru
        /// a identifia solutiile disponibile in backend; Metoda nu necesita nici un fel de corectii, poate fi utilizata ca atare.
        /// </summary>
        /// <returns></returns>
        [HttpGet("FelAlgoDisponibil")]
        public List<DescriereAlgoritm> FelAlgoDisponibil()
        {
            var raspuns = new List<DescriereAlgoritm>();
            string prefix = $"{Request.Scheme}://{Request.Host}{Request.Path}";
            int i = prefix.LastIndexOf('/');
            prefix = prefix.Substring(0, i);

            foreach(var m in typeof(JocController).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
            {
                var a = m.GetCustomAttribute<AlgoritmJocAttribute>();
                if (a != null)
                {
                    var h = m.GetCustomAttribute<HttpGetAttribute>();
                    if (h != null)
                    {
                        if (h.Template != null && !h.Template.Any(x => !(char.IsAscii(x) && char.IsLetterOrDigit(x))))
                        {
                            raspuns.Add(new DescriereAlgoritm
                            {
                                Descriere = "LOCALHOST: " + a.DenumireAlgoritm,
                                URL = prefix + "/" + h.Template
                            });
                        }
                    }
                }
            }

            return raspuns;
        }
    }
}