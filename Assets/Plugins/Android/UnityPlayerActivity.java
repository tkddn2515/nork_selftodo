package com.unity3d.player;

import android.app.Activity;
import android.content.Intent;
import android.content.res.Configuration;
import android.graphics.PixelFormat;
import android.os.Bundle;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.os.Process;

import android.content.Context;
import android.graphics.Color;
import android.text.InputFilter;
import android.text.InputType;
import android.text.TextWatcher;
import android.text.method.PasswordTransformationMethod;
import android.view.ViewGroup;
import android.view.inputmethod.EditorInfo;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.FrameLayout;
import android.text.Editable;
import android.view.LayoutInflater;
import android.widget.LinearLayout;
import android.view.Gravity;
import android.view.inputmethod.InputMethodManager;
import android.content.Context;
import android.graphics.Typeface;
import android.text.SpannableString;
import android.text.style.StyleSpan;
import android.view.KeyEvent;
import android.text.style.UnderlineSpan;
import android.widget.Toast;
import android.widget.TextView.OnEditorActionListener;
import android.text.Selection;
import android.os.Handler;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class UnityPlayerActivity extends Activity implements IUnityPlayerLifecycleEvents
{
    protected UnityPlayer mUnityPlayer; // don't change the name of this variable; referenced from native code
    private EditText et;
    private InputMethodManager imm;
    // Override this in your custom UnityPlayerActivity to tweak the command line arguments passed to the Unity Android Player
    // The command line arguments are passed as a string, separated by spaces
    // UnityPlayerActivity calls this from 'onCreate'
    // Supported: -force-gles20, -force-gles30, -force-gles31, -force-gles31aep, -force-gles32, -force-gles, -force-vulkan
    // See https://docs.unity3d.com/Manual/CommandLineArguments.html
    // @param cmdLine the current command line arguments, may be null
    // @return the modified command line string or null
    protected String updateUnityCommandLineArguments(String cmdLine)
    {
        return cmdLine;
    }

    // Setup activity layout
    @Override protected void onCreate(Bundle savedInstanceState)
    {
        requestWindowFeature(Window.FEATURE_NO_TITLE);
        super.onCreate(savedInstanceState);

        String cmdLine = updateUnityCommandLineArguments(getIntent().getStringExtra("unity"));
        getIntent().putExtra("unity", cmdLine);

        mUnityPlayer = new UnityPlayer(this, this);
        setContentView(mUnityPlayer);
        mUnityPlayer.requestFocus();

        int activity_sub_id = getResources().getIdentifier( "activity_sub", "layout", getPackageName());
        LayoutInflater inflater = (LayoutInflater)getSystemService(LAYOUT_INFLATER_SERVICE);
        FrameLayout ll = (FrameLayout)inflater.inflate(activity_sub_id, null);
        FrameLayout.LayoutParams p = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.MATCH_PARENT, FrameLayout.LayoutParams.MATCH_PARENT);
        addContentView(ll, p);

        FrameLayout fl = findViewById(R.id.fl);
        et = new EditText(getApplicationContext());
        LinearLayout.LayoutParams p2 = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WRAP_CONTENT, ViewGroup.LayoutParams.WRAP_CONTENT);
        et.setLayoutParams(p2);
        fl.addView(et);
        et.setBackgroundColor(Color.parseColor("#00000000"));
        et.setVisibility(View.GONE);

        et.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {
                UnityPlayer.UnitySendMessage("AndroidInputfield", "ReceiveMessage", ToJson("beforetextchanged", s.toString()));
            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {
                UnityPlayer.UnitySendMessage("AndroidInputfield", "ReceiveMessage", ToJson("textchanged", s.toString()));
            }

            @Override
            public void afterTextChanged(Editable arg0) {
                UnityPlayer.UnitySendMessage("AndroidInputfield", "ReceiveMessage", ToJson("aftertextchanged", arg0.toString()));
            }
        });

        et.setOnFocusChangeListener(new View.OnFocusChangeListener() {
            @Override
            public void onFocusChange(View view, boolean bFocus) {
                
                UnityPlayer.UnitySendMessage("AndroidInputfield", "ReceiveMessage", ToJson("focuschange", Boolean.toString(bFocus)));
            }
        });

        // et.setOnKeyListener(new View.OnKeyListener(){
        //     @Override
        //     public boolean onKey(View v, int keyCode, KeyEvent event){
        //         Toast.makeText(getApplicationContext(), keyCode + " : " + event.toString(), Toast.LENGTH_SHORT).show();
        //         if(event.getAction() == KeyEvent.ACTION_DOWN && keyCode == KeyEvent.KEYCODE_BACK){
        //             Toast.makeText(getApplicationContext(), et.getText().toString(), Toast.LENGTH_SHORT).show();
        //             return true;
        //         }
        //         return false;
        //     }
        // });

        et.setOnEditorActionListener(new TextView.OnEditorActionListener() {
            @Override
            public boolean onEditorAction(TextView view, int keyCode, KeyEvent event) {
                 UnityPlayer.UnitySendMessage("AndroidInputfield", "ReceiveMessage", ToJson("oneditoraction", "true"));
                 Hide();
                //Keypad에서 입력된 keycode, keyEvent값을 이용해서 처리해주기 위해 사용됨.
                //보통 Enter를 눌렀을때, 해당 동작을 실행해주기위해 사용.
                switch (keyCode) {
                    case EditorInfo.IME_ACTION_SEARCH:

                        break;
                    default:
                        // 기본 엔터키 동작
                        return false;
                }
                return true;
            }

        });

    }

    public String ToJson(String _key, String _value){
        String _tmp = "{\"keyboardAction\":\"";
        _tmp += _key;
        _tmp += "\",\"";
        _tmp += "data\":\"";
        _tmp += _value;
        _tmp += "\"}";
        return _tmp;
    }
    public void EditText(String ttext){
        runOnUiThread(new Runnable() {
            public void run() {
                EditText2(ttext);
            }
        });
    }


    public void EditText2(String jsonStr){

        String width = "0";
        String height = "0";
        String pos_x = "0";
        String pos_y = "0";
        String text = "";
        String text_size = "1";
        String text_color = "#ffffffff";
        String hint = "";
        String hint_color = "#ffffffff";
        String length = "0";
        String gravity_vertical = "1";
        String gravity_horizontal = "1";
        String inputtype = "normal";
        String imeoption = "done";
        String italic = "false";
        String underline = "false";
        String font = "none";
        String fontstyle = "normal";
        String padding_l = "0";
        String padding_r = "0";
        String padding_t = "0";
        String padding_b = "0";

        try {
            JSONArray jsonArray = new JSONArray(jsonStr);
            JSONObject jsonObject = jsonArray.getJSONObject(0);
            width = jsonObject.getString("width");
            height = jsonObject.getString("height");
            pos_x = jsonObject.getString("pos_x");
            pos_y = jsonObject.getString("pos_y");
            text = jsonObject.getString("text");
            text_size = jsonObject.getString("text_size");
            text_color = jsonObject.getString("text_color");
            hint = jsonObject.getString("hint");
            hint_color = jsonObject.getString("hint_color");
            length = jsonObject.getString("length");
            gravity_vertical = jsonObject.getString("gravity_vertical");
            gravity_horizontal = jsonObject.getString("gravity_horizontal");
            inputtype = jsonObject.getString("inputtype");
            imeoption = jsonObject.getString("imeoption");
            underline = jsonObject.getString("underline");
            font = jsonObject.getString("font");
            fontstyle = jsonObject.getString("fontstyle");
            padding_l = jsonObject.getString("padding_l");
            padding_r = jsonObject.getString("padding_r");
            padding_t = jsonObject.getString("padding_t");
            padding_b = jsonObject.getString("padding_b");
        }
        catch (JSONException e) {
            e.printStackTrace();
        }

        //위치
        //et.layout(500, 500, 50, 50);
        et.setTranslationX(Integer.parseInt(pos_x));
        et.setTranslationY(Integer.parseInt(pos_y));

        //text
        Typeface type = Typeface.createFromAsset(getAssets(),"font/" + font);

        switch (fontstyle) {
            case "normal":
            default:
                et.setTypeface(type, Typeface.NORMAL);
                break;
            case "bold":
                et.setTypeface(type, Typeface.BOLD);
                break;
            case "italic":
                et.setTypeface(type, Typeface.ITALIC);
                break;
            case "boldanditalic":
                et.setTypeface(type, Typeface.BOLD_ITALIC);
                break;
        }

        et.setText(text);
        et.setTextSize(Float.parseFloat(text_size));
        et.setTextColor(Color.parseColor(text_color));

        //길이 제한
        InputFilter[] EditFilter = new InputFilter[1];
        EditFilter[0] = new InputFilter.LengthFilter(Integer.parseInt(length));
        et.setFilters(EditFilter);

        //밑줄 없애기
        et.setBackground(null);

        //커서 유무
        et.setCursorVisible(true);

        
        //정렬
        int _vertical = Gravity.CENTER_VERTICAL;
        int _horizontal = Gravity.CENTER_HORIZONTAL;
        switch (gravity_vertical)
        {
            case "0":
                _vertical = Gravity.TOP;
                break;
            case "1":
                _vertical = Gravity.CENTER_VERTICAL;
                break;
            case "2":
                _vertical = Gravity.BOTTOM;
                break;
        }
        switch (gravity_horizontal)
        {
            case "0":
                _horizontal = Gravity.LEFT;
                break;
            case "1":
                _horizontal = Gravity.CENTER_HORIZONTAL;
                break;
            case "2":
                _horizontal = Gravity.RIGHT;
                break;
        }

        et.setGravity(_vertical|_horizontal);

       
        UnityPlayer.UnitySendMessage("AndroidInputfield", "ReceiveMessage", ToJson("back", "inputtype : " + inputtype));
        //인풋 타입
        switch (inputtype)
        {
            case "normal":
                et.setSingleLine();
                et.setInputType(InputType.TYPE_CLASS_TEXT);
                break;
            case "password":
                et.setSingleLine();
                et.setInputType(InputType.TYPE_TEXT_VARIATION_PASSWORD);
                et.setTransformationMethod(PasswordTransformationMethod.getInstance());
                break;
            case "number":
                et.setSingleLine();
                et.setInputType(InputType.TYPE_CLASS_NUMBER);
                break;
            case "numberDecimal":
                et.setSingleLine();
                et.setInputType(InputType.TYPE_NUMBER_FLAG_DECIMAL);
                break;
            case "personname":
                et.setSingleLine();
                et.setInputType(InputType.TYPE_TEXT_VARIATION_PERSON_NAME);
                break;
            case "email":
                et.setSingleLine();
                et.setInputType(InputType.TYPE_CLASS_TEXT|InputType.TYPE_TEXT_VARIATION_EMAIL_ADDRESS);
                break;
            case "datetime":
                et.setSingleLine();
                et.setInputType(InputType.TYPE_CLASS_DATETIME);
                break;
            case "multiline":
                et.setInputType(InputType.TYPE_TEXT_FLAG_MULTI_LINE);
                break;
            case "phone":
                et.setSingleLine();
                et.setInputType(InputType.TYPE_CLASS_PHONE);
                break;
        }
        
        switch (imeoption)
        {
            case "done":
                et.setImeOptions(EditorInfo.IME_ACTION_DONE);
                break;
            case "search":
                et.setImeOptions(EditorInfo.IME_ACTION_SEARCH);
                break;
            case "previous":
                et.setImeOptions(EditorInfo.IME_ACTION_PREVIOUS);
                break;
            case "next":
                et.setImeOptions(EditorInfo.IME_ACTION_NEXT);
                break;
            case "go":
                et.setImeOptions(EditorInfo.IME_ACTION_GO);
                break;
            case "send":
                et.setImeOptions(EditorInfo.IME_ACTION_SEND);
                break;
        }

        et.setPadding(Integer.parseInt(padding_l),Integer.parseInt(padding_t),Integer.parseInt(padding_r),Integer.parseInt(padding_b));

        //크기
        et.setWidth(Integer.parseInt(width));
        et.setHeight(Integer.parseInt(height));

        et.setVisibility(View.VISIBLE);

        et.requestFocus();

        if(imm == null){
            imm = (InputMethodManager) getSystemService(Context.INPUT_METHOD_SERVICE);
        }
        imm.showSoftInput(et, InputMethodManager.SHOW_IMPLICIT);

        et.setSelection(et.length());
    }

    public void RectChange(String ttext){
        runOnUiThread(new Runnable() {
            public void run() {
                RectChange2(ttext);
            }
        });
    }


    public void RectChange2(String jsonStr){

        //UnityPlayer.UnitySendMessage("AndroidInputfield", "ReceiveMessage", ToJson("oneditoraction", "true"));
        String width = "0";
        String height = "0";
        String pos_x = "0";
        String pos_y = "0";
        
        try {
            JSONArray jsonArray = new JSONArray(jsonStr);
            JSONObject jsonObject = jsonArray.getJSONObject(0);
            width = jsonObject.getString("width");
            height = jsonObject.getString("height");
            pos_x = jsonObject.getString("pos_x");
            pos_y = jsonObject.getString("pos_y");
            
        }
        catch (JSONException e) {
            e.printStackTrace();
        }
        //크기
        et.setWidth(Integer.parseInt(width));
        et.setHeight(Integer.parseInt(height));
        UnityPlayer.UnitySendMessage("AndroidInputfield", "ReceiveMessage", ToJson("back", "height2 : " + height));

        // //위치
        //et.layout(500, 500, 50, 50);
        et.setTranslationX(Integer.parseInt(pos_x));
        et.setTranslationY(Integer.parseInt(pos_y));
    }

    public void Hide(){
        runOnUiThread(new Runnable() {
            public void run() {
                Hide2();
            }
        });
    }

    public void Hide2(){
        et.setVisibility(View.GONE);
        if(imm != null){
            imm.hideSoftInputFromWindow(et.getWindowToken(), 0);
            UnityPlayer.UnitySendMessage("AndroidInputfield", "ReceiveMessage", ToJson("hide", "true"));
        }
    }

    // When Unity player unloaded move task to background
    @Override public void onUnityPlayerUnloaded() {
        moveTaskToBack(true);
    }

    // Callback before Unity player process is killed
    @Override public void onUnityPlayerQuitted() {
    }

    @Override protected void onNewIntent(Intent intent)
    {
        // To support deep linking, we need to make sure that the client can get access to
        // the last sent intent. The clients access this through a JNI api that allows them
        // to get the intent set on launch. To update that after launch we have to manually
        // replace the intent with the one caught here.
        setIntent(intent);
        mUnityPlayer.newIntent(intent);
    }

    // Quit Unity
    @Override protected void onDestroy ()
    {
        mUnityPlayer.destroy();
        super.onDestroy();
    }

    // If the activity is in multi window mode or resizing the activity is allowed we will use
    // onStart/onStop (the visibility callbacks) to determine when to pause/resume.
    // Otherwise it will be done in onPause/onResume as Unity has done historically to preserve
    // existing behavior.
    @Override protected void onStop()
    {
        super.onStop();

        if (!MultiWindowSupport.getAllowResizableWindow(this))
            return;

        mUnityPlayer.pause();
    }

    @Override protected void onStart()
    {
        super.onStart();

        if (!MultiWindowSupport.getAllowResizableWindow(this))
            return;

        mUnityPlayer.resume();
    }

    // Pause Unity
    @Override protected void onPause()
    {
        super.onPause();

        if (MultiWindowSupport.getAllowResizableWindow(this))
            return;

        mUnityPlayer.pause();
    }

    // Resume Unity
    @Override protected void onResume()
    {
        super.onResume();

        if (MultiWindowSupport.getAllowResizableWindow(this))
            return;

        mUnityPlayer.resume();
    }

    // Low Memory Unity
    @Override public void onLowMemory()
    {
        super.onLowMemory();
        mUnityPlayer.lowMemory();
    }

    // Trim Memory Unity
    @Override public void onTrimMemory(int level)
    {
        super.onTrimMemory(level);
        if (level == TRIM_MEMORY_RUNNING_CRITICAL)
        {
            mUnityPlayer.lowMemory();
        }
    }

    // This ensures the layout will be correct.
    @Override public void onConfigurationChanged(Configuration newConfig)
    {
        super.onConfigurationChanged(newConfig);
        mUnityPlayer.configurationChanged(newConfig);
    }

    // Notify Unity of the focus change.
    @Override public void onWindowFocusChanged(boolean hasFocus)
    {
        super.onWindowFocusChanged(hasFocus);
        mUnityPlayer.windowFocusChanged(hasFocus);
    }

    // For some reason the multiple keyevent type is not supported by the ndk.
    // Force event injection by overriding dispatchKeyEvent().
    @Override public boolean dispatchKeyEvent(KeyEvent event)
    {
        if (event.getAction() == KeyEvent.ACTION_MULTIPLE)
            return mUnityPlayer.injectEvent(event);
        return super.dispatchKeyEvent(event);
    }

    // Pass any events not handled by (unfocused) views straight to UnityPlayer
    @Override public boolean onKeyUp(int keyCode, KeyEvent event)     { return mUnityPlayer.injectEvent(event); }
    @Override public boolean onKeyDown(int keyCode, KeyEvent event)   { return mUnityPlayer.injectEvent(event); }
    @Override public boolean onTouchEvent(MotionEvent event)          { return mUnityPlayer.injectEvent(event); }
    /*API12*/ public boolean onGenericMotionEvent(MotionEvent event)  { return mUnityPlayer.injectEvent(event); }
}
