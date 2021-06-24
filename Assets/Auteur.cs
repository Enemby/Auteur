using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
public class Auteur : MonoBehaviour
{
    string myOutput = "";
    string myPath;
    // Start is called before the first frame update
    void getLaunchOptions()
    {//Stubbed for later
        string[] args = Environment.GetCommandLineArgs();
    }
    void Start()
    {
        myPath = Application.dataPath;
        myOutput = ReadDirectory(myPath);
        SaveSource();
        CompileCredits();
    }
    public void CompileCredits()
    {//Searches for .atr files, and reads them.
        string myDir = ReadDirectory(Application.dataPath);
        string[] myFiles = myDir.Split('\n');
        List<string> atrFiles = new List<string>();
        foreach(var atr in myFiles)
        {
            if (atr.Contains(".atr") && !atr.Contains(".meta"))
            {//Filter out meta cause it'll break everything
                atrFiles.Add(atr);
            }
        }
        string[] myReturn = atrFiles.ToArray();
        string[] myCredits = GetCredits(myReturn);

        File.WriteAllText(myPath + "/credits.txt", ""); //Clear file
        StreamWriter writer = new StreamWriter(myPath + "/credits.txt", true);
        writer.WriteLine("Using work by: \n");
        foreach(var author in myCredits)
        {
            writer.WriteLine(author);
        }
        writer.WriteLine("");
        writer.WriteLine("Credits compiled dynamically by Auteur, a author crediting tool made by Enemby.");
        writer.Close();
    }
    public void CompileAuteur()
    { //Stubbed. Eventually this should create automatic .atr files based on UI info.

    }
    string[] GetCredits(string[] theFiles)
    {
        List<String> credits = new List<string>();
        foreach(var file in theFiles)
        {
            StreamReader reader = new StreamReader(file);
            string myLine = reader.ReadLine();
            if(myLine.Contains("Credit: "))
            {
                myLine = myLine.Replace("Credit: ", "");
                credits.Add(myLine);
            }
        }
        return credits.ToArray();
    }
    string ReadDirectory(string path)
    {
        string dirOutput = "";
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo f in info)
        {
            dirOutput += f.ToString();
            dirOutput += "\n";
        }
        return dirOutput;
    }
    void SaveSource()
    { //Write down everything we found and decided
        File.WriteAllText(myPath + "/Output.txt", ""); //Clear file
        StreamWriter writer = new StreamWriter(myPath + "/Output.txt", true);
        writer.Write("Files Found: \n");
        writer.Write(myOutput);
        writer.Close();
        Debug.Log(myOutput);
    }

    // Update is called once per frame
    void Update()
    {
        myPath = GetComponent<AuteurUI>().selectedDir;
    }
}
