using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{

   public InputField input;
   public InputField name2;

   private DB db;

   void Start()
   {
       db = GetComponent<DB>();
   }

   public void Button()
   {
      StartCoroutine(db.LoadData(input.text));
   }

  public void ButtonScene2()
   {
     db.changeage(input.text);
   }
   public void SaveDataB()
   {
     db.SaveData(name2.text);
   }
   
    public void changeProductF()
   {
     db.changeProductF(input.text);
   }
   
    public void changeProductC()
   {
     db.changeProductC(input.text);
   }
    public void AllUsers()
   {
      StartCoroutine(db.GetLeaders());
   }



}