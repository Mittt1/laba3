open System
open System.IO

// Функция для подсчета файлов, имена которых начинаются с заданного символа
let countname (directory: string) (char: char) =
    Directory.EnumerateFiles(directory)
    |> Seq.filter (fun file -> 
        let Name = Path.GetFileName(file)
        Name.Length > 0 && Char.ToLower(Name.[0]) = Char.ToLower(char))
    |> Seq.length

// Функция для чтения ввода с клавиатуры, возвращающая последовательность строк
let readkeyboard (prompt: string) (invalidMessage: string) =
    seq {
        while true do
            printf "%s: " prompt
            let input = Console.ReadLine()
            if String.IsNullOrEmpty(input) then
                printfn "%s" invalidMessage
            else
                yield input
    }

// Функция для чтения символа с проверкой
let read_s_proverkoy (prompt: string) =
    readkeyboard prompt "Введите корректный символ"
    |> Seq.filter (fun input -> input.Length = 1)
    |> Seq.map (fun input -> input.[0])

// Функция для чтения пути к каталогу с проверкой и выводом ошибки
let readdirectory (prompt: string) =
    readkeyboard prompt "Введите корректный путь к каталогу"
    |> Seq.map (fun input ->
        if Directory.Exists(input) then
            Some input
        else
            printfn "Ошибка: Каталог '%s' не существует. Попробуйте снова." input
            None)
    |> Seq.filter Option.isSome
    |> Seq.map Option.get

// Основная программа
[<EntryPoint>]
let main argv =
    try
        printfn "Программа для вывода количества файлов, имя которых начинается"
        printfn "с заданного символа, в указанном каталоге (без подкаталогов)." 
        printfn " "
        // Ввод символа
        printfn "Введите символ для поиска файлов:"
        let znachenie = read_s_proverkoy "Ожидаю"
        let char = Seq.head znachenie

        // Ввод каталога
        printfn "\nВведите путь к каталогу для поиска:"
        let directory0 = readdirectory "Ожидаю"
        let directory = Seq.head directory0

        // Подсчет файлов
        let count = countname directory char
        printfn $"\nВ каталоге '{directory}' найдено {count} файлов, начинающихся с '{char}'"

        0 // Успешное завершение программы
    with
    | :? UnauthorizedAccessException ->
        printfn "Ошибка: Нет доступа к указанной директории."
        1
    | :? IOException as ex ->
        printfn $"Ошибка ввода-вывода: {ex.Message}"
        1
    | ex ->
        printfn $"Произошла ошибка: {ex.Message}"
        1