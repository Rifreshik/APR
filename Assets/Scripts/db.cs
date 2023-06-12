using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public class DB : MonoBehaviour
{
    DatabaseReference dbRef;
    FirebaseAuth auth;
    FirebaseApp app;
    public Text text;
    public InputField input;
    public InputField ProductFI;
    public InputField ProductCI;
    [SerializeField] Text TextLeaders;

    

    public InputField emailInput;
    public InputField passwordInput;
    public GameObject login;
    public GameObject Main;

    void Start()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;
        
    }

   public async void SignIn()
{
    string email = emailInput.text;
    string password = passwordInput.text;

    try
    {
        await auth.SignInWithEmailAndPasswordAsync(email, password);
        
        login.SetActive(false);
        Main.SetActive(true);
    }
    catch (Exception e)
    {
        Debug.LogError(e.Message);
    }
}

    public void SaveData(string str)
    {
        if (dbRef == null)
        {
            Debug.LogError("dbRef is null");
            return;
        }
        User user = new User(str, 17, "offline", 0, 0 );
        string json = JsonUtility.ToJson(user);
        dbRef.Child("users").Child(str).SetRawJsonValueAsync(json);
    }

    public class User
    {
        public string name;
        public int age;
        public string status;
        public int ProductF;
        public int ProductC;

        public User(string name, int age, string status, int ProductF,int ProductC)
        {
            this.name = name;
            this.age = age;
            this.status = status;
            this.ProductF = ProductF;
            this.ProductF = ProductC;
        }
    }

    public IEnumerator LoadData(string str)
    {
        var user = dbRef.Child("users").Child(str).GetValueAsync();

        yield return new WaitUntil(predicate: () => user.IsCompleted);

        if (user.Exception != null)
        {
            Debug.LogError(user.Exception);
        }
        else if (user.Result == null)
        {
            Debug.LogError("Нету такого пользователя ");
        }
        else
        {
            DataSnapshot snapshot = user.Result;
            text.text = snapshot.Child("age").Value.ToString() + " " + snapshot.Child("name").Value.ToString();
        }
    }

    public void changeage(string str)
    {
        var user = dbRef.Child("users").Child(str).GetValueAsync();
        user.ContinueWith(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError(task.Exception);
            }
            else
            {
                DataSnapshot snapshot = task.Result;
                int age = int.Parse(snapshot.Child("age").Value.ToString());
                int newValue = age - int.Parse(input.text);
                dbRef.Child("users").Child(str).Child("age").SetValueAsync(newValue.ToString())
                    .ContinueWith(task2 =>
                    {
                        if (task2.Exception != null)
                        {
                            Debug.LogError(task2.Exception);
                        }
                        else
                        {
                            Debug.Log("Age value updated successfully!");
                        }
                    });
            }
        });
    }



    public void changeProductF(string str)
    {
        var user = dbRef.Child("users").Child(str).GetValueAsync();
        user.ContinueWith(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError(task.Exception);
            }
            else
            {
                DataSnapshot snapshot = task.Result;
                int ProductF = int.Parse(snapshot.Child("ProductF").Value.ToString());
               int newValue = ProductF + int.Parse(ProductFI.text);
                dbRef.Child("users").Child(str).Child("ProductF").SetValueAsync(newValue.ToString())
                    .ContinueWith(task2 =>
                    {
                        if (task2.Exception != null)
                        {
                            Debug.LogError(task2.Exception);
                        }
                        else
                        {
                            Debug.Log("ProductF value updated successfully!");
                        }
                    });
            }
        }); 
    }
   
 public void changeProductC(string str)
    {
        var user = dbRef.Child("users").Child(str).GetValueAsync();
        user.ContinueWith(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError(task.Exception);
            }
            else
            {
                DataSnapshot snapshot = task.Result;
                int ProductC = int.Parse(snapshot.Child("ProductC").Value.ToString());
               int newValue = ProductC + int.Parse(ProductCI.text);
               Debug.Log(newValue);
                dbRef.Child("users").Child(str).Child("ProductC").SetValueAsync(newValue.ToString())


                    .ContinueWith(task2 =>
                    {
                        if (task2.Exception != null)
                        {
                            Debug.LogError(task2.Exception);
                        }
                        else
                        {
                            Debug.Log("ProductC value updated successfully!");
                        }
                    });
            }
        }); 
    }
public IEnumerator GetLeaders()
{
    Debug.Log("Начинаю выполнение функции GetLeaders.");

    var leaders = dbRef.Child("users").OrderByChild("age").GetValueAsync();

    yield return new WaitUntil(predicate: () => leaders.IsCompleted);

    if (leaders.Exception != null)
    {
        Debug.LogError("Ошибка при получении данных о лидерах!");
    }
    else if (leaders.Result.Value == null)
    {
        Debug.LogError("Данные о лидерах не найдены!");
    }
    else
    {
        DataSnapshot snapshot = leaders.Result;
        foreach (DataSnapshot dataChildSnapshot in snapshot.Children.Reverse())
        {
            string name = dataChildSnapshot.Child("name").Value.ToString();
            string age = dataChildSnapshot.Child("age").Value.ToString();
            Debug.Log($"Добавляю запись: {name}:{age}");
            TextLeaders.text += "\n" + name + ":" + age;
        }
    }

    Debug.Log("Выполнение функции GetLeaders завершено.");
}



}
  

