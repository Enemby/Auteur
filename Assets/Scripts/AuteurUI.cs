using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class AuteurUI : MonoBehaviour
{
    public Text myText;
    public string selectedDir = "";
    public string myAuthor = "";
    public Auteur myInstance;
    public Text logText;
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class OpenDialogFile
    {
        //Found at: https://www.programmersought.com/article/25424410433/
        //Original source: https://blog.csdn.net/cwj649956781/article/details/76218218
        //This code is NOT Open Source. I 'Enemby' didn't make it and have no right to distribute it, so I'll probably remove it from the repo.
        //TODO: Find or make new version with a less suspect origin
        public int structSize = 0;
        public IntPtr dlgOwner = IntPtr.Zero;
        public IntPtr instance = IntPtr.Zero;
        public String filter = null;
        public String customFilter = null;
        public int maxCustFilter = 0;
        public int filterIndex = 0;
        public String file = null;
        public int maxFile = 0;
        public String fileTitle = null;
        public int maxFileTitle = 0;
        public String initialDir = null;
        public String title = null;
        public int flags = 0;
        public short fileOffset = 0;
        public short fileExtension = 0;
        public String defExt = null;
        public IntPtr custData = IntPtr.Zero;
        public IntPtr hook = IntPtr.Zero;
        public String templateName = null;
        public IntPtr reservedPtr = IntPtr.Zero;
        public int reservedInt = 0;
        public int flagsEx = 0;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class OpenDialogDir
    {
        public IntPtr hwndOwner = IntPtr.Zero;
        public IntPtr pidlRoot = IntPtr.Zero;
        public String pszDisplayName = null;
        public String lpszTitle = null;
        public UInt32 ulFlags = 0;
        public IntPtr lpfn = IntPtr.Zero;
        public IntPtr lParam = IntPtr.Zero;
        public int iImage = 0;
    }

    public class DllOpenFileDialog
    {
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern bool GetOpenFileName([In, Out] OpenDialogFile ofn);

        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern bool GetSaveFileName([In, Out] OpenDialogFile ofn);

        [DllImport("shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SHBrowseForFolder([In, Out] OpenDialogDir ofn);

        [DllImport("shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern bool SHGetPathFromIDList([In] IntPtr pidl, [In, Out] char[] fileName);

    }



    public void QQ()
    {
        OpenDialogDir ofn2 = new OpenDialogDir();
        ofn2.pszDisplayName = new string(new char[2000]); ; // store directory path buffer    
        ofn2.lpszTitle = "Open Project";//Title    
                                        //ofn2.ulFlags = BIF_NEWDIALOGSTYLE | BIF_EDITBOX; // New style with edit box    
        IntPtr pidlPtr = DllOpenFileDialog.SHBrowseForFolder(ofn2);

        char[] charArray = new char[2000];
        for (int i = 0; i < 2000; i++)
            charArray[i] = '\0';

        DllOpenFileDialog.SHGetPathFromIDList(pidlPtr, charArray);
        string fullDirPath = new String(charArray);


        fullDirPath = fullDirPath.Substring(0, fullDirPath.IndexOf('\0'));
        selectedDir = fullDirPath;

        Debug.Log(fullDirPath);//This is the selected directory path.  
    }
    private void Start()
    {
        selectedDir = Application.dataPath;
        InvokeRepeating("updateLog", 2.0f, 1.0f);
    }
    private void Update()
    {
        myText.text = selectedDir;
    }
    public void setAuthor(string author)
    {
        myAuthor = author;
        if (myInstance != null)
        {
            myInstance.myAuthor = myAuthor;
        }
    }
    public void updateLog()
    {
        string[] myLines = File.ReadAllLines("log.txt");
        logText.text = myLines[myLines.Length-1];
    }
}