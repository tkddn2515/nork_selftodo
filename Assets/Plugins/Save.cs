using UnityEngine;
using System.Collections;
using System.Xml; 
using System.Xml.Serialization; 
using System.IO; 
using System.Text;
using System.Reflection;
using System;
using System.Security;
using System.Security.Cryptography;

public class Save : MonoBehaviour
{
	public static void SavePos(string name,GameObject gameObject)
	{
		PlayerPrefs.SetString(name,gameObject.transform.position.x + "&" +gameObject.transform.position.y + "&" +gameObject.transform.position.z);
	}
	public static void SaveString(string name,string variable)
	{
		PlayerPrefs.SetString(name,variable);
	}
	public static void SaveStringArray(string name, string[] variable)
    {
		PlayerPrefs.SetString(name, String.Join("#"[0].ToString(), variable));
    }
	public static void SaveStringArray(string name, string[] variable, char separator)
    {
		PlayerPrefs.SetString(name, String.Join(separator.ToString(), variable));
    }
	public static void SaveInt(string name,int variable)
	{
		PlayerPrefs.SetInt(name,variable);
	}
	public static void SaveIntArray(string name,int[] variable)
    {
		string a = "";
        for (int i = 0; i < variable.Length; i++)
		{a += variable[i].ToString() + "#";}
        PlayerPrefs.SetString(name, a.ToString());
    }
	public static void SaveUInt(string name,uint variable)
	{
		PlayerPrefs.SetString(name,variable.ToString());
	}
	public static void SaveUIntArray(string name,uint[] variable)
    {
		string a = "";
        for (int i = 0; i < variable.Length; i++)
		{a += variable[i].ToString() + "#";}
        PlayerPrefs.SetString(name, a.ToString());
    }
	public static void SaveLong(string name,long variable)
	{
		PlayerPrefs.SetString(name,variable.ToString());
	}
	public static void SaveLongArray(string name,long[] variable)
    {
		string a = "";
        for (int i = 0; i < variable.Length; i++)
		{a += variable[i].ToString() + "#";}
        PlayerPrefs.SetString(name, a.ToString());
    }
	public static void SaveULong(string name,ulong variable)
	{
		PlayerPrefs.SetString(name,variable.ToString());
	}
	public static void SaveULongArray(string name,ulong[] variable)
    {
		string a = "";
        for (int i = 0; i < variable.Length; i++)
		{a += variable[i].ToString() + "#";}
        PlayerPrefs.SetString(name, a.ToString());
    }
	public static void SaveShort(string name,short variable)
	{
		PlayerPrefs.SetString(name,variable.ToString());
	}
	public static void SaveShortArray(string name,short[] variable)
    {
		string a = "";
        for (int i = 0; i < variable.Length; i++)
		{a += variable[i].ToString() + "#";}
        PlayerPrefs.SetString(name, a.ToString());
    }
	public static void SaveUShort(string name,ushort variable)
	{
		PlayerPrefs.SetString(name,variable.ToString());
	}
	public static void SaveUShortArray(string name,ushort[] variable)
    {
		string a = "";
        for (int i = 0; i < variable.Length; i++)
		{a += variable[i].ToString() + "#";}
        PlayerPrefs.SetString(name, a.ToString());
    }
	public static void SaveFloat(string name,float variable)
	{
		PlayerPrefs.SetFloat(name,variable);
	}
	public static void SaveFloatArray(string name,float[] variable)
    {
		string a = "";
        for (int i = 0; i < variable.Length; i++)
		{a += variable[i].ToString() + "#";}
        PlayerPrefs.SetString(name, a.ToString());
    }
	public static void SaveDouble(string name,double variable)
	{
		PlayerPrefs.SetString(name,variable.ToString());
	}
	public static void SaveDoubleArray(string name,double[] variable)
    {
		string a = "";
        for (int i = 0; i < variable.Length; i++)
		{a += variable[i].ToString() + "#";}
        PlayerPrefs.SetString(name, a.ToString());
    }
	public static void SaveBool(string name,bool variable)
	{
		PlayerPrefs.SetString(name,variable.ToString());
	}
	public static void SaveBoolArray(string name,bool[] variable)
    {
		string a = "";
        for (int i = 0; i < variable.Length; i++)
		{a += variable[i].ToString() + "#";}
        PlayerPrefs.SetString(name, a.ToString());
    }
	public static void SaveChar(string name,char variable)
	{
		PlayerPrefs.SetString(name,variable.ToString());
	}
	public static void SaveCharArray(string name, char[] variable)
    {
		string a = "";
        for (int i = 0; i < variable.Length; i++)
		{a += variable[i].ToString() + "#";}
        PlayerPrefs.SetString(name, a.ToString());
    }
	public static void SaveCharArray(string name, char[] variable, char separator)
    {
		string a = "";
        for (int i = 0; i < variable.Length; i++)
		{a += variable[i].ToString() + separator.ToString();}
        PlayerPrefs.SetString(name, a.ToString());
    }
	public static void SaveDecimal(string name,decimal variable)
	{
		PlayerPrefs.SetString(name,variable.ToString());
	}
	public static void SaveDecimalArray(string name,decimal[] variable)
    {
		string a = "";
        for (int i = 0; i < variable.Length; i++)
		{a += variable[i].ToString() + "#";}
        PlayerPrefs.SetString(name, a.ToString());
    }
	public static void SaveByte(string name,byte variable)
	{
		PlayerPrefs.SetString(name,variable.ToString());
	}
	public static void SaveByteArray(string name,byte[] variable)
    {
		string a = "";
        for (int i = 0; i < variable.Length; i++)
		{a += variable[i].ToString() + "#";}
        PlayerPrefs.SetString(name, a.ToString());
    }
	public static void SaveSByte(string name,sbyte variable)
	{
		PlayerPrefs.SetString(name,variable.ToString());
	}
	public static void SaveSByteArray(string name,sbyte[] variable)
    {
		string a = "";
        for (int i = 0; i < variable.Length; i++)
		{a += variable[i].ToString() + "#";}
        PlayerPrefs.SetString(name, a.ToString());
    }
	public static void SaveVector4(string name,Vector4 variable)
	{
		PlayerPrefs.SetString(name,variable.x + "&" + variable.y + "&" + variable.z + "&" + variable.w);
	}
	public static void SaveVector4Array(string name,Vector4[] variable)
	{
		string a = "";
        for (int i = 0; i < variable.Length; i++)
		{a += variable[i].x + "&" + variable[i].y + "&" + variable[i].z + "&" + variable[i].w + "#";}
        PlayerPrefs.SetString(name, a.ToString());
	}
	public static void SaveVector3(string name,Vector3 variable)
	{
		PlayerPrefs.SetString(name,variable.x + "&" +variable.y + "&" +variable.z);
	}
	public static void SaveVector3Array(string name,Vector3[] variable)
	{
		string a = "";
        for (int i = 0; i < variable.Length; i++)
		{a += variable[i].x + "&" + variable[i].y + "&" + variable[i].z + "#";}
        PlayerPrefs.SetString(name, a.ToString());
	}
	public static void SaveVector2(string name,Vector2 variable)
	{
		PlayerPrefs.SetString(name,variable.x + "&" +variable.y);
	}
	public static void SaveVector2Array(string name,Vector2[] variable)
	{
		string a = "";
        for (int i = 0; i < variable.Length; i++)
		{a += variable[i].x + "&" + variable[i].y + "#";}
        PlayerPrefs.SetString(name, a.ToString());
	}
	public static void SaveQuaternion(string name,Quaternion variable)
	{
		PlayerPrefs.SetString(name,variable.x + "&" + variable.y + "&" + variable.z + "&" + variable.w);
	}
	public static void SaveQuaternionArray(string name,Quaternion[] variable)
	{
		string a = "";
        for (int i = 0; i < variable.Length; i++)
		{a += variable[i].x + "&" + variable[i].y + "&" + variable[i].z + "&" + variable[i].w + "#";}
        PlayerPrefs.SetString(name, a.ToString());
	}
	public static void SaveColor(string name,Color variable)
	{
		PlayerPrefs.SetString(name,variable.r.ToString() + "&" + variable.g.ToString() + "&" + variable.b.ToString() + "&" + variable.a.ToString());
	}
	public static void SaveColorArray(string name,Color[] variable)
	{
		string a = "";
        for (int i = 0; i < variable.Length; i++)
		{a += variable[i].r.ToString() + "&" + variable[i].g.ToString() + "&" + variable[i].b.ToString() + "&" + variable[i].a.ToString() + "#";}
        PlayerPrefs.SetString(name, a.ToString());
	}
	public static void SaveKeyCode(string name,KeyCode variable)
	{
		PlayerPrefs.SetString(name,variable.ToString());
	}
	public static void SaveKeyCodeArray(string name,KeyCode[] variable)
    {
		string a = "";
        for (int i = 0; i < variable.Length; i++)
		{a += variable[i].ToString() + "#";}
        PlayerPrefs.SetString(name, a.ToString());
    }
	public static void SaveRect(string name,Rect variable)
	{
		PlayerPrefs.SetString(name,variable.x.ToString() + "&" + variable.y.ToString() + "&" + variable.width.ToString() + "&" + variable.height.ToString());
	}
	public static void SaveRectArray(string name,Rect[] variable)
	{
		string a = "";
        for (int i = 0; i < variable.Length; i++)
		{a += variable[i].x.ToString() + "&" + variable[i].y.ToString() + "&" + variable[i].width.ToString() + "&" + variable[i].height.ToString() + "#";}
        PlayerPrefs.SetString(name, a.ToString());
	}
	public static void Delete(string name)
	{
		PlayerPrefs.DeleteKey(name);
	}
	public static void DeleteAll()
	{
		PlayerPrefs.DeleteAll();
	}
}