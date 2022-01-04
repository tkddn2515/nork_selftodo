using UnityEngine;
using System.Collections;
using System.Xml; 
using System.Xml.Serialization; 
using System.IO; 
using System.Text;
using System.Reflection;
using System;

public class Load : MonoBehaviour
{
	public static void LoadPos(string name,GameObject gameObject)
	{
		if (!PlayerPrefs.HasKey(name))
		{gameObject.transform.position = new Vector3(0,0,0);}
		
		string[] a = PlayerPrefs.GetString(name).Split("&"[0]);
		Vector3 b = new Vector3(float.Parse(a[0]),float.Parse(a[1]),float.Parse(a[2]));
		gameObject.transform.position = b;
	}
	public static string LoadString(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return "";}
		string a = PlayerPrefs.GetString(name);
		return a;
	}
	public static string[] LoadStringArray(string name)
    {
		if (!PlayerPrefs.HasKey(name))
		{return null;}
		string[] a = PlayerPrefs.GetString(name).Split("#"[0]);
		return a;
    }
	public static string[] LoadStringArray(string name, char separator)
    {
		if (!PlayerPrefs.HasKey(name))
		{return null;}
		string[] a = PlayerPrefs.GetString(name).Split(separator);
		return a;
    }
	public static int LoadInt(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return 0;}
		int a = PlayerPrefs.GetInt(name);
		return a;
	}
	public static int[] LoadIntArray(string name)
    {
		if (!PlayerPrefs.HasKey(name))
		{return null;}
		string[] a = PlayerPrefs.GetString(name).Split("#"[0]);
		int[] b = new int[a.Length - 1];
		for (int i = 0; i < a.Length - 1; i++)
		{b[i] = Convert.ToInt32(a[i]);}
		return b;
    }
	public static uint LoadUInt(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return 0;}
		uint a = uint.Parse(PlayerPrefs.GetString(name));
		return a;
	}
	public static uint[] LoadUIntArray(string name)
    {
		if (!PlayerPrefs.HasKey(name))
		{return null;}
		string[] a = PlayerPrefs.GetString(name).Split("#"[0]);
		uint[] b = new uint[a.Length - 1];
		for (int i = 0; i < a.Length - 1; i++)
		{b[i] = Convert.ToUInt32(a[i]);}
		return b;
    }
	public static long LoadLong(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return 0;}
		long a = long.Parse(PlayerPrefs.GetString(name));
		return a;
	}
	public static long[] LoadLongArray(string name)
    {
		if (!PlayerPrefs.HasKey(name))
		{return null;}
		string[] a = PlayerPrefs.GetString(name).Split("#"[0]);
		long[] b = new long[a.Length - 1];
		for (int i = 0; i < a.Length - 1; i++)
		{b[i] = long.Parse(a[i]);}
		return b;
    }
	public static ulong LoadULong(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return 0;}
		ulong a = ulong.Parse(PlayerPrefs.GetString(name));
		return a;
	}
	public static ulong[] LoadULongArray(string name)
    {
		if (!PlayerPrefs.HasKey(name))
		{return null;}
		string[] a = PlayerPrefs.GetString(name).Split("#"[0]);
		ulong[] b = new ulong[a.Length - 1];
		for (int i = 0; i < a.Length - 1; i++)
		{b[i] = ulong.Parse(a[i]);}
		return b;
    }
	public static short LoadShort(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return 0;}
		short a = short.Parse(PlayerPrefs.GetString(name));
		return a;
	}
	public static short[] LoadShortArray(string name)
    {
		if (!PlayerPrefs.HasKey(name))
		{return null;}
		string[] a = PlayerPrefs.GetString(name).Split("#"[0]);
		short[] b = new short[a.Length - 1];
		for (int i = 0; i < a.Length - 1; i++)
		{b[i] = short.Parse(a[i]);}
		return b;
    }
	public static ushort LoadUShort(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return 0;}
		ushort a = ushort.Parse(PlayerPrefs.GetString(name));
		return a;
	}
	public static ushort[] LoadUShortArray(string name)
    {
		if (!PlayerPrefs.HasKey(name))
		{return null;}
		string[] a = PlayerPrefs.GetString(name).Split("#"[0]);
		ushort[] b = new ushort[a.Length - 1];
		for (int i = 0; i < a.Length - 1; i++)
		{b[i] = ushort.Parse(a[i]);}
		return b;
    }
	public static float LoadFloat(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return 0;}
		float a = PlayerPrefs.GetFloat(name);
		return a;
	}
	public static float[] LoadFloatArray(string name)
    {
		if (!PlayerPrefs.HasKey(name))
		{return null;}
		string[] a = PlayerPrefs.GetString(name).Split("#"[0]);
		float[] b = new float[a.Length - 1];
		for (int i = 0; i < a.Length - 1; i++)
		{b[i] = float.Parse(a[i]);}
		return b;
    }
	public static double LoadDouble(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return 0;}
		double a = double.Parse(PlayerPrefs.GetString(name));
		return a;
	}
	public static double[] LoadDoubleArray(string name)
    {
		if (!PlayerPrefs.HasKey(name))
		{return null;}
		string[] a = PlayerPrefs.GetString(name).Split("#"[0]);
		double[] b = new double[a.Length - 1];
		for (int i = 0; i < a.Length - 1; i++)
		{b[i] = double.Parse(a[i]);}
		return b;
    }
	public static bool LoadBool(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return false;}
		string a = PlayerPrefs.GetString(name);
		return bool.Parse(a);
	}
	public static bool[] LoadBoolArray(string name)
    {
		if (!PlayerPrefs.HasKey(name))
		{return null;}
		string[] a = PlayerPrefs.GetString(name).Split("#"[0]);
		bool[] b = new bool[a.Length - 1];
		for (int i = 0; i < a.Length - 1; i++)
		{b[i] = bool.Parse(a[i]);}
		return b;
    }
	public static char LoadChar(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return char.Parse("");}
		char a = char.Parse(PlayerPrefs.GetString(name));
		return a;
	}
	public static char[] LoadCharArray(string name)
    {
		if (!PlayerPrefs.HasKey(name))
		{return null;}
		string[] a = PlayerPrefs.GetString(name).Split("#"[0]);
		char[] b = new char[a.Length - 1];
		for (int i = 0; i < a.Length - 1; i++)
		{b[i] = char.Parse(a[i]);}
		return b;
    }
	public static decimal LoadDecimal(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return 0;}
		decimal a = decimal.Parse(PlayerPrefs.GetString(name));
		return a;
	}
	public static decimal[] LoadDecimalArray(string name)
    {
		if (!PlayerPrefs.HasKey(name))
		{return null;}
		string[] a = PlayerPrefs.GetString(name).Split("#"[0]);
		decimal[] b = new decimal[a.Length - 1];
		for (int i = 0; i < a.Length - 1; i++)
		{b[i] = decimal.Parse(a[i]);}
		return b;
    }
	public static byte LoadByte(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return 0;}
		byte a = byte.Parse(PlayerPrefs.GetString(name));
		return a;
	}
	public static byte[] LoadByteArray(string name)
    {
		if (!PlayerPrefs.HasKey(name))
		{return null;}
		string[] a = PlayerPrefs.GetString(name).Split("#"[0]);
		byte[] b = new byte[a.Length - 1];
		for (int i = 0; i < a.Length - 1; i++)
		{b[i] = byte.Parse(a[i]);}
		return b;
    }
	public static sbyte LoadSByte(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return 0;}
		sbyte a = sbyte.Parse(PlayerPrefs.GetString(name));
		return a;
	}
	public static sbyte[] LoadSByteArray(string name)
    {
		if (!PlayerPrefs.HasKey(name))
		{return null;}
		string[] a = PlayerPrefs.GetString(name).Split("#"[0]);
		sbyte[] b = new sbyte[a.Length - 1];
		for (int i = 0; i < a.Length - 1; i++)
		{b[i] = sbyte.Parse(a[i]);}
		return b;
    }
	public static Vector4 LoadVector4(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return new Vector4(0,0,0,0);}
		string[] a = PlayerPrefs.GetString(name).Split("&"[0]);
		Vector4 b = new Vector4(float.Parse(a[0]),float.Parse(a[1]),float.Parse(a[2]),float.Parse(a[3]));
		return b;
	}
	public static Vector4[] LoadVector4Array(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return null;}
		string[] a = PlayerPrefs.GetString(name).Split("#"[0]);
		Vector4[] b = new Vector4[a.Length - 1];
		for (int i = 0; i < a.Length - 1; i++)
		{string[] c = a[i].Split("&"[0]);
		b[i] = new Vector4(float.Parse(c[0]),float.Parse(c[1]),float.Parse(c[2]),float.Parse(c[3]));}
		return b;
	}
	public static Vector3 LoadVector3(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return new Vector3(0,0,0);}
		string[] a = PlayerPrefs.GetString(name).Split("&"[0]);
		Vector3 b = new Vector3(float.Parse(a[0]),float.Parse(a[1]),float.Parse(a[2]));
		return b;
	}
	public static Vector3[] LoadVector3Array(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return null;}
		string[] a = PlayerPrefs.GetString(name).Split("#"[0]);
		Vector3[] b = new Vector3[a.Length - 1];
		for (int i = 0; i < a.Length - 1; i++)
		{string[] c = a[i].Split("&"[0]);
		b[i] = new Vector3(float.Parse(c[0]),float.Parse(c[1]),float.Parse(c[2]));}
		return b;
	}
	public static Vector2 LoadVector2(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return new Vector2(0,0);}
		string[] a = PlayerPrefs.GetString(name).Split("&"[0]);
		Vector2 b = new Vector2(float.Parse(a[0]),float.Parse(a[1]));
		return b;
	}
	public static Vector2[] LoadVector2Array(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return null;}
		string[] a = PlayerPrefs.GetString(name).Split("#"[0]);
		Vector2[] b = new Vector2[a.Length - 1];
		for (int i = 0; i < a.Length - 1; i++)
		{string[] c = a[i].Split("&"[0]);
		b[i] = new Vector2(float.Parse(c[0]),float.Parse(c[1]));}
		return b;
	}
	public static Quaternion LoadQuaternion(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return new Quaternion(0,0,0,0);}
		string[] a = PlayerPrefs.GetString(name).Split("&"[0]);
		Quaternion b = new Quaternion(float.Parse(a[0]),float.Parse(a[1]),float.Parse(a[2]),float.Parse(a[3]));
		return b;
	}
	public static Quaternion[] LoadQuaternionArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return null;}
		string[] a = PlayerPrefs.GetString(name).Split("#"[0]);
		Quaternion[] b = new Quaternion[a.Length - 1];
		for (int i = 0; i < a.Length - 1; i++)
		{string[] c = a[i].Split("&"[0]);
		b[i] = new Quaternion(float.Parse(c[0]),float.Parse(c[1]),float.Parse(c[2]),float.Parse(c[3]));}
		return b;
	}
	public static Color LoadColor(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return new Color(0,0,0,0);}
		string[] a = PlayerPrefs.GetString(name).Split("&"[0]);
		Color b = new Color(float.Parse(a[0]),float.Parse(a[1]),float.Parse(a[2]),float.Parse(a[3]));
		return b;
	}
	public static Color[] LoadColorArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return null;}
		string[] a = PlayerPrefs.GetString(name).Split("#"[0]);
		Color[] b = new Color[a.Length - 1];
		for (int i = 0; i < a.Length - 1; i++)
		{string[] c = a[i].Split("&"[0]);
		b[i] = new Color(float.Parse(c[0]),float.Parse(c[1]),float.Parse(c[2]),float.Parse(c[3]));}
		return b;
	}
	public static KeyCode LoadKeyCode(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return KeyCode.Space;}
		return (KeyCode)Enum.Parse( typeof(KeyCode), PlayerPrefs.GetString(name));
	}
	public static KeyCode[] LoadKeyCodeArray(string name)
    {
		if (!PlayerPrefs.HasKey(name))
		{return null;}
		string[] a = PlayerPrefs.GetString(name).Split("#"[0]);
		KeyCode[] b = new KeyCode[a.Length - 1];
		for (int i = 0; i < a.Length - 1; i++)
		{b[i] = (KeyCode)Enum.Parse( typeof(KeyCode), a[i]);}
		return b;
    }
	public static Rect LoadRect(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return new Rect(0,0,0,0);}
		string[] a = PlayerPrefs.GetString(name).Split("&"[0]);
		Rect b = new Rect(float.Parse(a[0]),float.Parse(a[1]),float.Parse(a[2]),float.Parse(a[3]));
		return b;
	}
	public static Rect[] LoadRectArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return null;}
		string[] a = PlayerPrefs.GetString(name).Split("#"[0]);
		Rect[] b = new Rect[a.Length - 1];
		for (int i = 0; i < a.Length - 1; i++)
		{string[] c = a[i].Split("&"[0]);
		b[i] = new Rect(float.Parse(c[0]),float.Parse(c[1]),float.Parse(c[2]),float.Parse(c[3]));}
		return b;
	}
	public static Texture2D LoadTexture2D(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{return null;}
		string[] a = PlayerPrefs.GetString(name).Split("&"[0]);
		byte[] bytes = Convert.FromBase64String(a[2]);
		Texture2D b = new Texture2D(int.Parse(a[0]),int.Parse(a[1]));
		b.LoadImage(bytes);
  		return b;
	}
}
