Imports System.IO

Module Module1
    Dim DogCard(30) As String
    Dim exercise(30) As Integer
    Dim intelligence(30) As Integer
    Dim friendliness(30) As Integer
    Dim drool(30) As Integer
    Dim pCards As New List(Of Integer)
    Dim cpuCards As New List(Of Integer)
    Function DisplayError(code As Integer)
        Console.Clear()
        Console.ForegroundColor = ConsoleColor.Red
        Console.Write("Error: ")
        Console.ForegroundColor = ConsoleColor.White
        Select Case code
            Case 1
                Console.Write("Please enter a number!")
            Case 2
                Console.Write("Please enter a valid number! (1, 2)")
            Case 3
                Console.Write("Please enter a valid number between 4 and 30! Make sure it's even!")
            Case 4
                Console.Write("Please enter a valid number! (1 - 4)")
        End Select
        Console.WriteLine()
        System.Threading.Thread.Sleep(900)
        Return False
    End Function


    Sub Main()
        Console.Clear()
        Console.Title = "Dog Top Trumps"
        Console.WriteLine("    Dog Top Trumps")
        Console.WriteLine("----------------------")
        Console.WriteLine("Press any key to start")
        Console.ReadKey()
        MainMenu()
    End Sub

    Sub MainMenu()
        Dim inputValid As Boolean = False
        Dim debugMode As Boolean = False
        Dim mCho As Integer = 0
        While Not inputValid
                Console.Clear()
                Console.WriteLine("Main Menu")
                Console.WriteLine()
                Console.WriteLine("-------------")
                Console.WriteLine("1. Play Game")
                Console.WriteLine("2. Quit")
                Console.WriteLine("-------------")
                Console.WriteLine()
                Console.Write("Please enter your menu choice using its respective number: ")
                Dim userChoice As String = Console.ReadLine()
                If IsNumeric(userChoice) Then
                    mCho = userChoice
                    inputValid = True
                    Select Case mCho
                        Case 1
                            PlayGame(debugMode)
                        Case 2
                            Console.Clear()
                            Console.WriteLine("Celebrity Dogs will now close.")
                            System.Threading.Thread.Sleep(1000)
                            Environment.Exit(0)
                        Case Else
                            inputValid = DisplayError(2)
                    End Select
                ElseIf userChoice = "ACTIVATE_DEBUG" Then
                    debugMode = True
                    Console.Clear()
                    Console.Write("Debug: ")
                    Console.ForegroundColor = ConsoleColor.Green
                    Console.Write("ACTIVE")
                    Console.WriteLine()
                    Console.ForegroundColor = ConsoleColor.White
                    System.Threading.Thread.Sleep(2000)
                ElseIf userChoice = "DEACTIVATE_DEBUG" Then
                    debugMode = False
                    Console.Clear()
                    Console.Write("Debug: ")
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.Write("DISABLED")
                    Console.WriteLine()
                    Console.ForegroundColor = ConsoleColor.White
                    System.Threading.Thread.Sleep(2000)
                Else
                    inputValid = DisplayError(1)
                End If
            End While
    End Sub

    Sub PlayGame(debugMode As Boolean)
        Dim lastRoundWinner As String = "p"
        Dim winnerDecided As Boolean = False
        Dim winner As String = ""
        Dim cardsInPlay As Integer = EnterCardNumber()
        ReadFile(cardsInPlay)
        RandomiseStats(cardsInPlay)
        While winnerDecided = False

            DisplayTopCard()
            If debugMode = True Then
                DisplayCPUsCard()
            End If
            Dim nextCategory As Integer = ChooseNextCategory(lastRoundWinner, debugMode)
            ComparativeTable(nextCategory)
            Console.WriteLine("Press any key to continue")
            Console.ReadKey(True)
            Console.Clear()
            lastRoundWinner = CompareStats(nextCategory)
            Select Case lastRoundWinner
                Case "p"
                    Console.WriteLine("As such, you will receive the card CPU used last round")
                    Console.WriteLine("Both this and your top card will go to the bottom of your deck.")
                    Console.WriteLine("You will also be allowed to choose the next category to be played from.")
                    Console.WriteLine("Press any key to continue")
                Case "c"
                    Console.WriteLine("Unfortunately, the CPU won this round.")
                    Console.WriteLine("As such, it will receive the card you were using last round.")
                    Console.WriteLine("That card and the card it was using will go to the bottom of its deck.")
                    Console.WriteLine("The CPU will also choose the next category to be played.")
                    Console.WriteLine("Press any key to continue")
                Case "DRAW"
                    Console.WriteLine("The last round was a draw, and so you will win the round.")
                    Console.WriteLine("As such, you will receive the card CPU used last round")
                    Console.WriteLine("Both this and your top card will go to the bottom of your deck.")
                    Console.WriteLine("You will also be allowed to choose the next category to be played from.")
                    Console.WriteLine("Press any key to continue")
            End Select
            Console.ReadKey(True)
            Console.Clear()
            MoveCardsToCorrectPlaces(lastRoundWinner)
            winner = WinDetection()
            If winner = "" Then
                winnerDecided = False
            Else
                winnerDecided = True
            End If
        End While
        If winner = "p" Then
            YouWin()
        ElseIf winner = "c" Then
            YouLose()
        End If
    End Sub

    Function EnterCardNumber()
        Dim inputValid As Boolean
        Dim cardsInPlay As Integer = 0
        While Not inputValid
                Console.Clear()
                Console.WriteLine("Please enter how many cards you wish to be in play.")
                Console.WriteLine("Keep in mind that both you and the computer will get half this amount of cards.")
                Console.WriteLine("Make sure you input a number between 4 and 30, inclusively! This also means that")
                Console.WriteLine("the number you input should be even, so that the cards are even.")
                Console.WriteLine()
                Dim userInput As String = Console.ReadLine()
                If IsNumeric(userInput) Then
                    cardsInPlay = userInput
                    inputValid = True
                    If cardsInPlay >= 4 And cardsInPlay Mod 2 = 0 And cardsInPlay <= 30 Then
                        inputValid = True
                        Console.Clear()
                    Else
                        inputValid = DisplayError(3)
                    End If
                Else
                inputValid = DisplayError(1)
            End If
            End While
            Return cardsInPlay
    End Function

    Sub ReadFile(cardsInPlay As Integer)
        Dim filePath As String = System.IO.Path.GetFullPath("dogs.txt")
        Using fileReader As StreamReader = New StreamReader(filePath)
            Dim line As String = ""
            Dim i As Integer = 0
            While Not fileReader.EndOfStream
                line = fileReader.ReadLine
                If i = cardsInPlay Then
                    Exit While
                End If
                If i Mod 2 = 0 Then
                    pCards.Add(i)
                ElseIf i Mod 2 = 1 Then
                    cpuCards.Add(i)
                End If
                DogCard(i) = line
                i = i + 1
            End While
        End Using
    End Sub

    Sub RandomiseStats(cardsInPlay As Integer)
        Dim randomStats As Random = New Random()
        For i As Integer = 0 To cardsInPlay
            exercise(i) = randomStats.Next(1, 6) ' max integer is exclusive for some reason
            intelligence(i) = randomStats.Next(1, 101) ' max integer is exclusive for some reason
            friendliness(i) = randomStats.Next(1, 11) ' max integer is exclusive for some reason
            drool(i) = randomStats.Next(1, 11) ' max integer is exclusive for some reason
        Next
    End Sub

    Sub DisplayTopCard()
        Console.WriteLine()
        Console.WriteLine("Dog: {0}", DogCard(pCards(0)))
        Console.WriteLine("--------------------")
        Console.WriteLine("Exercise:        {0}", exercise(pCards(0)))
        Console.WriteLine("--------------------")
        Console.WriteLine("Intelligence:    {0}", intelligence(pCards(0)))
        Console.WriteLine("--------------------")
        Console.WriteLine("Friendliness:    {0}", friendliness(pCards(0)))
        Console.WriteLine("--------------------")
        Console.WriteLine("Drool:           {0}", drool(pCards(0)))
        Console.WriteLine("--------------------")
    End Sub

    Sub DisplayCPUsCard()
        Console.WriteLine()
        Console.WriteLine("Dog: {0}", DogCard(cpuCards(0)))
        Console.WriteLine("--------------------")
        Console.WriteLine("Exercise:        {0}", exercise(cpuCards(0)))
        Console.WriteLine("--------------------")
        Console.WriteLine("Intelligence:    {0}", intelligence(cpuCards(0)))
        Console.WriteLine("--------------------")
        Console.WriteLine("Friendliness:    {0}", friendliness(cpuCards(0)))
        Console.WriteLine("--------------------")
        Console.WriteLine("Drool:           {0}", drool(cpuCards(0)))
        Console.WriteLine("--------------------")
    End Sub

    Function ChooseNextCategory(lastRoundWinner As String, debugMode As Boolean)
        Dim nextCategory As Integer = 0
        Dim inputValid As Boolean = False
        If lastRoundWinner = "p" Or lastRoundWinner = "DRAW" Then
            While inputValid = False
                Console.WriteLine()
                Console.WriteLine("Please enter which category you wish to play with as a number:")
                Console.WriteLine()
                Console.WriteLine("1. Exercise")
                Console.WriteLine("2. Intelligence")
                Console.WriteLine("3. Friendliness")
                Console.WriteLine("4. Drool")
                Console.WriteLine()
                Dim userInput As String = Console.ReadLine()
                If IsNumeric(userInput) Then
                    nextCategory = userInput
                    If nextCategory = 1 Or nextCategory = 2 Or nextCategory = 3 Or nextCategory = 4 Then
                        inputValid = True
                    Else
                        inputValid = DisplayError(4)
                        Console.Clear()
                        DisplayTopCard()
                        If debugMode = True Then
                            DisplayCPUsCard()
                        End If
                    End If
                Else
                    inputValid = DisplayError(1)
                    Console.Clear()
                    DisplayTopCard()
                    If debugMode = True Then
                        DisplayCPUsCard()
                    End If
                End If
            End While
        ElseIf lastRoundWinner = "c" Then
            Dim randomCategory As Random = New Random()
            nextCategory = randomCategory.Next(1, 5) ' max integer is exclusive for some reason
        End If
        Console.Clear()
        Return nextCategory
    End Function

    Sub ComparativeTable(nextCategory As Integer)
        Console.Write("Category:        ")
        Console.Write("{0}    ", DogCard(pCards(0)))
        Console.Write("{0}", DogCard(cpuCards(0)))
        Console.WriteLine()
        Console.WriteLine("-------------------------------")
        If nextCategory = 1 Then
            Console.ForegroundColor = ConsoleColor.DarkYellow
            Console.Write("Exercise:         ")
            Console.Write("{0}        ", exercise(pCards(0)))
            Console.Write("{0}   ", exercise(cpuCards(0)))
            Console.WriteLine()
            Console.ForegroundColor = ConsoleColor.White
        Else
            Console.Write("Exercise:         ")
            Console.Write("{0}        ", exercise(pCards(0)))
            Console.Write("{0}   ", exercise(cpuCards(0)))
            Console.WriteLine()
        End If
        Console.WriteLine("-------------------------------")
        If nextCategory = 2 Then
            Console.ForegroundColor = ConsoleColor.DarkYellow
            Console.Write("Intelligence:     ")
            Console.Write("{0}       ", intelligence(pCards(0)))
            Console.Write("{0}   ", intelligence(cpuCards(0)))
            Console.WriteLine()
            Console.ForegroundColor = ConsoleColor.White
        Else
            Console.Write("Intelligence:     ")
            Console.Write("{0}       ", intelligence(pCards(0)))
            Console.Write("{0}   ", intelligence(cpuCards(0)))
            Console.WriteLine()
        End If
        Console.WriteLine("-------------------------------")
        If nextCategory = 3 Then
            Console.ForegroundColor = ConsoleColor.DarkYellow
            Console.Write("Friendliness:     ")
            Console.Write("{0}        ", friendliness(pCards(0)))
            Console.Write("{0}   ", friendliness(cpuCards(0)))
            Console.WriteLine()
            Console.ForegroundColor = ConsoleColor.White
        Else
            Console.Write("Friendliness:     ")
            Console.Write("{0}        ", friendliness(pCards(0)))
            Console.Write("{0}   ", friendliness(cpuCards(0)))
            Console.WriteLine()
        End If
        Console.WriteLine("-------------------------------")
        If nextCategory = 4 Then
            Console.ForegroundColor = ConsoleColor.DarkYellow
            Console.Write("Drool:            ")
            Console.Write("{0}        ", drool(pCards(0)))
            Console.Write("{0}   ", drool(cpuCards(0)))
            Console.WriteLine()
            Console.ForegroundColor = ConsoleColor.White
        Else
            Console.Write("Drool:            ")
            Console.Write("{0}        ", drool(pCards(0)))
            Console.Write("{0}   ", drool(cpuCards(0)))
            Console.WriteLine()
        End If
        Console.WriteLine("-------------------------------")
    End Sub

    Function CompareStats(nextCategory As Integer)
        Dim lastRoundWinner As String = ""
        If nextCategory = 1 Then
            If exercise(pCards(0)) > exercise(cpuCards(0)) Then
                lastRoundWinner = "p"
            ElseIf exercise(pCards(0)) < exercise(cpuCards(0)) Then
                lastRoundWinner = "c"
            Else
                lastRoundWinner = "DRAW"
            End If
        ElseIf nextCategory = 2 Then
            If intelligence(pCards(0)) > intelligence(cpuCards(0)) Then
                lastRoundWinner = "p"
            ElseIf intelligence(pCards(0)) < intelligence(cpuCards(0)) Then
                lastRoundWinner = "c"
            Else
                lastRoundWinner = "DRAW"
            End If
        ElseIf nextCategory = 3 Then
            If friendliness(pCards(0)) > friendliness(cpuCards(0)) Then
                lastRoundWinner = "p"
            ElseIf friendliness(pCards(0)) < friendliness(cpuCards(0)) Then
                lastRoundWinner = "c"
            Else
                lastRoundWinner = "DRAW"
            End If
        ElseIf nextCategory = 4 Then
            If drool(pCards(0)) < drool(cpuCards(0)) Then
                lastRoundWinner = "p"
            ElseIf drool(pCards(0)) > drool(cpuCards(0)) Then
                lastRoundWinner = "c"
            Else
                lastRoundWinner = "DRAW"
            End If
        End If
        Return lastRoundWinner
    End Function

    Sub MoveCardsToCorrectPlaces(lastRoundWinner As String)
        Dim temp As New List(Of Integer)
        If lastRoundWinner = "p" Or lastRoundWinner = "DRAW" Then
            temp.Add(pCards(0))
            pCards = pCards.Except(temp).ToList
            pCards.Add(temp(0))
            temp.RemoveAt(0)
            temp.Add(cpuCards(0))
            cpuCards = cpuCards.Except(temp).ToList
            pCards.Add(temp(0))
            temp.RemoveAt(0)
        ElseIf lastRoundWinner = "c" Then
            temp.Add(cpuCards(0))
            cpuCards = cpuCards.Except(temp).ToList
            cpuCards.Add(temp(0))
            temp.RemoveAt(0)
            temp.Add(pCards(0))
            pCards = pCards.Except(temp).ToList()
            cpuCards.Add(temp(0))
            temp.RemoveAt(0)
        End If
    End Sub

    Function WinDetection()
        Dim winner As String = ""
        If pCards.Count = 0 Then
            winner = "c"
        ElseIf cpuCards.Count = 0 Then
            winner = "p"
        End If
        Return winner
    End Function

    Sub YouWin()
        Dim userPlayAgain = False
        While userPlayAgain = False
            Console.Clear()
            Console.WriteLine()
            Console.WriteLine("Congratulations! You won the game")
            Console.WriteLine()
            Console.WriteLine("Would you like to play again? (Y/N)")
            Dim playAgain As String = Console.ReadLine()
            playAgain = playAgain.ToUpper
            If playAgain = "Y" Then
                userPlayAgain = True
                Main()
            ElseIf playAgain = "N" Then
                userPlayAgain = False
                Console.Clear()
                Console.WriteLine("Thank you for playing!")
                System.Threading.Thread.Sleep(1000)
                Environment.Exit(0)
            Else
                userPlayAgain = False
                Console.Clear()
                Console.ForegroundColor = ConsoleColor.Red
                Console.Write("Error: ")
                Console.ForegroundColor = ConsoleColor.White
                Console.Write("Please enter a valid letter! (Y/N)")
                Console.WriteLine()
                System.Threading.Thread.Sleep(2000)
            End If
        End While
    End Sub

    Sub YouLose()
        Dim userPlayAgain = False
        While userPlayAgain = False
            Console.Clear()
            Console.WriteLine()
            Console.WriteLine("Ouch! Seems like you didn't win this time.")
            Console.WriteLine("Better luck next time!")
            Console.WriteLine()
            Console.WriteLine("Would you like to play again? (Y/N)")
            Dim playAgain As String = Console.ReadLine()
            playAgain = playAgain.ToUpper
            If playAgain = "Y" Then
                userPlayAgain = True
                Main()
            ElseIf playAgain = "N" Then
                userPlayAgain = False
                Console.Clear()
                Console.WriteLine("Thank you for playing!")
                System.Threading.Thread.Sleep(1000)
                Environment.Exit(0)
            Else
                userPlayAgain = False
                Console.Clear()
                Console.ForegroundColor = ConsoleColor.Red
                Console.Write("Error: ")
                Console.ForegroundColor = ConsoleColor.White
                Console.Write("Please enter a valid letter! (Y/N)")
                Console.WriteLine()
                System.Threading.Thread.Sleep(2000)
            End If
        End While
    End Sub

End Module
