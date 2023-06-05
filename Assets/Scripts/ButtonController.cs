using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
   public InputField input;
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

}
