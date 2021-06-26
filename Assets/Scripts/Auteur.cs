using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
public class Auteur : MonoBehaviour
{
    public string myAuthor = "Enemby";
    string myOutput = "";
    string myPath;
    // Start is called before the first frame update
    void getLaunchOptions()
    {//Stubbed for later
        string[] args = Environment.GetCommandLineArgs();
        WriteToLog("Found Launch Options: ");
        for(int i = 0;i < args.Length;i++)
        {
            if (args[i].StartsWith("-"))
            {
                WriteToLog("" + args[i]);
            }
            if(args[i] == "-compilepath")
            {
                myPath = args[i] + 1;
            }
            if(args[i] == "-authorname")
            {
                myAuthor = args[i + 1];
            }
            if(args[i] == "-compilecredits")
            {
                CompileCredits();
            }
            if(args[i] == "-compileauthor")
            {
                CompileAuteur();
            }
        }
    }
    void Start()
    {
        myPath = Application.dataPath;
        WriteToLog("reset");
        getLaunchOptions();
        WriteToLog("Set default path to: " + myPath);
    }
    public void CompileCredits()
    {//Searches for .atr files, and reads them.
        WriteToLog("Compiling Credits at: " + myPath);
        SaveSource();
        string myDir = ReadDirectory(myPath, "*.atr");
        string[] myFiles = myDir.Split('\n');
        if (myFiles.Length > 0) { 
            List<string> atrFiles = new List<string>();
            foreach (var atr in myFiles)
            {
                if (atr.Contains(".atr") && !atr.Contains(".meta"))
                {//Filter out meta cause it'll break everything
                    atrFiles.Add(atr);
                }
            }
            string[] myReturn = atrFiles.ToArray();
            string[] myCredits = GetCredits(myReturn);
            if (myReturn.Length != 0)
            {
                File.WriteAllText(myPath + "/credits.txt", ""); //Clear file
                StreamWriter writer = new StreamWriter(myPath + "/credits.txt", true);
                writer.WriteLine("Using work by: \n");
                foreach (var author in myCredits)
                {
                    writer.WriteLine(author);
                }
                writer.WriteLine("");
                writer.WriteLine("Credits compiled dynamically by Auteur, a author crediting tool made by Enemby.");
                writer.Close();
                WriteToLog("credits.txt has been populated.");
            }
            else
            {
                WriteToLog("No ATRs Found!Canceled Credits!");
            }
        }
    }
    public void CompileAuteur()
    { //Stubbed. Eventually this should create automatic .atr files based on UI info.
        WriteToLog("Creating .ATR at: " + myPath);
        WriteToLog("File is: " + myAuthor + ".atr");
        string files = ReadRelativeDirectory(myPath, ".");
        File.WriteAllText(myPath + "/" + myAuthor + ".atr", "");
        StreamWriter writer = new StreamWriter(myPath + "/"+myAuthor+".atr", true);
        writer.WriteLine("Credit: " + myAuthor);
        writer.WriteLine("");
        writer.WriteLine("Files: ");
        writer.Write(files);
        writer.Close();
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
    string ReadDirectory(string path,string myPattern)
    {
        string dirOutput = "";
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles(myPattern,SearchOption.AllDirectories);
        foreach (FileInfo f in info)
        {
            dirOutput += f.ToString();
            dirOutput += "\n";
        }
        return dirOutput;
    }
    void WriteToLog(string toWrite)
    {
        if (toWrite == "reset")
        {
            File.WriteAllText("log.txt", ""); //Clear the log
        }
        else
        {
            StreamWriter writer = new StreamWriter("log.txt", true);
            writer.WriteLine(toWrite);
            writer.Close();
        }
    }
    string ReadRelativeDirectory(string path, string myPattern)
    {
        string dirOutput = "";
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles(myPattern, SearchOption.AllDirectories);
        foreach (FileInfo f in info)
        {
            string myFilePath = f.ToString();
            string[] myFile = myFilePath.Split('\\');

            dirOutput += myFile[myFile.Length-1];
            dirOutput += "\n";
        }
        return dirOutput;
    }
    void SaveSource()
    { //Write down everything we found and decided
        WriteToLog("Output.txt populated");
        myOutput = ReadDirectory(myPath, ".");
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
