try { Console.WindowWidth = 160; } catch { /* Ignora se o sistema não permitir */ }


// DATASET E MATRIZ DE INCIDÊNCIA
string[] alunos = {
    "Ryan", "Igor", "Gabriel", "Lucas", "José",
    "Gustavo", "Carlos", "Gabi", "Leo", "Julia"
};

string[] atributos = { "Clean Code", "Matemática", "Soft Skills", "Projeto" };

int[,] incidencia = {
    { 1, 1, 0, 0 },
    { 1, 0, 0, 1 },
    { 0, 1, 1, 0 },
    { 0, 0, 1, 1 },
    { 1, 1, 1, 0 },
    { 0, 1, 0, 1 },
    { 1, 0, 1, 0 },
    { 0, 0, 0, 1 },
    { 1, 1, 0, 0 },
    { 0, 1, 1, 1 }
};

// Gerar matrizes derivadas
int[,] transposta = CalcularTransposta(incidencia);
int[,] similaridade = MultiplicarMatrizes(incidencia, transposta);
ZerarDiagonal(similaridade);

int[,] coocorrencia = MultiplicarMatrizes(transposta, incidencia);
ZerarDiagonal(coocorrencia);


// MENU
int opcao = -1;

while (opcao != 0)
{
    Console.Clear();
    Console.WriteLine("==== MENU ====");
    Console.WriteLine("1 - Ver MATRIZ DE INCIDÊNCIA");
    Console.WriteLine("2 - Ver MATRIZ DE SIMILARIDADE");
    Console.WriteLine("3 - Ver MATRIZ DE COOCORRÊNCIA");
    Console.WriteLine("0 - Sair");
    Console.Write("\nEscolha uma opção: ");

    if (!int.TryParse(Console.ReadLine(), out opcao))
        opcao = -1;

    switch (opcao)
    {
        case 1:
            Console.Clear();
            Console.WriteLine("=== 1. MATRIZ DE INCIDÊNCIA (Alunos x Atributos) ===");
            ImprimirMatriz(incidencia, alunos, atributos);
            Console.WriteLine("\nPressione ENTER para voltar ao menu...");
            Console.ReadLine();
            break;

        case 2:
            Console.Clear();
            Console.WriteLine("=== 2. MATRIZ DE SIMILARIDADE (Quem parece com quem) ===");
            ImprimirMatriz(similaridade, alunos, alunos);
            CalcularMetricasTopologicas(similaridade, alunos.Length, "Similaridade");
            Console.WriteLine("\nPressione ENTER para voltar ao menu...");
            Console.ReadLine();
            break;

        case 3:
            Console.Clear();
            Console.WriteLine("=== 3. MATRIZ DE COOCORRÊNCIA (Quais temas aparecem juntos) ===");
            ImprimirMatriz(coocorrencia, atributos, atributos);
            CalcularMetricasTopologicas(coocorrencia, atributos.Length, "Coocorrência");
            Console.WriteLine("\nPressione ENTER para voltar ao menu...");
            Console.ReadLine();
            break;

        case 0:
            Console.WriteLine("Saindo...");
            break;

        default:
            Console.WriteLine("Opção inválida!");
            Thread.Sleep(1000);
            break;
    }
}


// mostrar as Matrizes
static void ImprimirMatriz(int[,] matriz, string[] labelsLinha, string[] labelsCol)
{
    int larguraCol1 = 0;
    foreach (var l in labelsLinha) if (l.Length > larguraCol1) larguraCol1 = l.Length;
    larguraCol1 += 3; 

    int larguraDados = 0;
    foreach (var l in labelsCol) if (l.Length > larguraDados) larguraDados = l.Length;
    if (larguraDados < 3) larguraDados = 3; 
    larguraDados += 2; 

    Console.Write("".PadRight(larguraCol1));
    foreach (var header in labelsCol)
    {
        Console.Write(CentralizarTexto(header, larguraDados) + "|");
    }
    Console.WriteLine();

    // Linha divisória
    Console.WriteLine(new string('-', larguraCol1 + (larguraDados + 1) * labelsCol.Length));

    // Imprimir Linhas
    for (int i = 0; i < matriz.GetLength(0); i++)
    {
        Console.Write(labelsLinha[i].PadRight(larguraCol1));

        for (int j = 0; j < matriz.GetLength(1); j++)
        {
            string valor = matriz[i, j].ToString();
            Console.Write(CentralizarTexto(valor, larguraDados) + "|");
        }
        Console.WriteLine();
    }
}

static string CentralizarTexto(string texto, int largura)
{
    if (texto.Length >= largura) return texto;
    int espacoLivre = largura - texto.Length;
    int padEsquerda = espacoLivre / 2 + texto.Length;
    return texto.PadLeft(padEsquerda).PadRight(largura);
}

// Calcula a Transposta
static int[,] CalcularTransposta(int[,] matriz)
{
    int linhas = matriz.GetLength(0); // aqui pega quantas linhas a matriz original tem
    int cols = matriz.GetLength(1);   // aqui pega quantas colunas ela tem

    int[,] transposta = new int[cols, linhas]; // aqui cria uma nova matriz invertendo linha por coluna

    for (int i = 0; i < linhas; i++)           // aqui começa a andar pelas linhas da matriz original
        for (int j = 0; j < cols; j++)         // aqui anda pelas colunas dela
            transposta[j, i] = matriz[i, j];   // aqui é a troca: o que era linha vira coluna

    return transposta; // aqui devolve a matriz transposta pronta
}

// Multiplica as Matrizes
static int[,] MultiplicarMatrizes(int[,] matrizA, int[,] matrizB)
{
    int linhasA = matrizA.GetLength(0); // pega quantas linhas tem a matriz A
    int colsA = matrizA.GetLength(1);   // pega quantas colunas tem a matriz A
    int colsB = matrizB.GetLength(1);   // pega quantas colunas tem a matriz B

    int[,] resultado = new int[linhasA, colsB]; // aqui cria a matriz final já com o tamanho certinho

    for (int i = 0; i < linhasA; i++)           // aqui percorre cada linha da matriz A
    {
        for (int j = 0; j < colsB; j++)         // aqui percorre cada coluna da matriz B
        {
            int soma = 0;                       // aqui vai acumulando o valor da multiplicação

            for (int k = 0; k < colsA; k++)     
                soma += matrizA[i, k] * matrizB[k, j]; // aqui faz a multiplicação (linha x coluna)

            resultado[i, j] = soma;             // aqui coloca o resultado na posição final da matriz
        }
    }
    return resultado; // devolve a matriz multiplicada
}

// a multiplicacao acontece assim
//A linha 2:    [a b c d]
//B coluna 3:   [w
//               x
//               y
//               z]

//resultado[2, 3] = a * w + b * x + c * y + d * z


static void ZerarDiagonal(int[,] matriz)
{
    for (int i = 0; i < matriz.GetLength(0); i++)  // aqui anda pela diagonal da matriz
        matriz[i, i] = 0;                          // aqui zera o valor da diagonal (evita contar com ele mesmo)
}

// Calcula as Metricas Topologicas
static void CalcularMetricasTopologicas(int[,] matrizAdjacencia, int numNos, string nomeGrafo) 
{
    Console.WriteLine($"\n--- Métricas do Grafo de {nomeGrafo} ---");
    int somaGraus = 0;

    for (int i = 0; i < numNos; i++)
    {
        int grauNo = 0;
        for (int j = 0; j < numNos; j++)
            if (matrizAdjacencia[i, j] > 0) grauNo += matrizAdjacencia[i, j];
        // Exibir grau de forma compacta se forem muitos nós, ou detalhada:
        somaGraus += grauNo;
    }
    Console.WriteLine($"Soma total dos Graus: {somaGraus}");

    int arestas = somaGraus / 2;
    double grauMedio = (double)somaGraus / numNos;
    double densidade = (2.0 * arestas) / (numNos * (numNos - 1));

    Console.WriteLine($"-> Grau Médio: {grauMedio:F2}");
    Console.WriteLine($"-> Densidade: {densidade:F4}");
    Console.WriteLine("---------------------------------------------");
}