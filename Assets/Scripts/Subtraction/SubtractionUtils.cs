using UnityEngine;

/*
    SubtractUtils:
     - Contains helper functions for Subtraction Game

     Written by: Jaiden Clee [2025]

*/
public static class SubtractionUtils
{
    /*
        Generates a random subtraction question
        Returns a tuple
    */
    public static (string questionText, int answer) GenerateQuestion()
    {
        int a = Random.Range(0, 50);
        int b = Random.Range(0, 50);

        //No negative numbers
        if (b > a)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        string question = $"{a} - {b} = ";
        int correctAnswer = a - b;

        Debug.Log($"Current Correct Answer = {correctAnswer}");

        return (question, correctAnswer);
    }

    public static bool IsAnswerCorrect(string playerInput, int correctAnswer)
    {
        if (int.TryParse(playerInput, out int parsedAnswer))
        {
            return parsedAnswer == correctAnswer;
        }
        return false; //consider invalid input as incorrect
    }
}
