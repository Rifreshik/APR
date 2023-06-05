using UnityEngine;
using Firebase.Database;
using System.Collections;
using UnityEngine.UI;
using Firebase;

public class DB : MonoBehaviour
{
    DatabaseReference dbRef;
    FirebaseApp app;
    public Text text;
    public InputField input;


    void Start()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

    
    }

    public void SaveData(string str)
    {
        User  user = new User (str, 17, "offline");
        string json = JsonUtility.ToJson(user);
        dbRef.Child("users").Child(str). SetRawJsonValueAsync(json);
    }

    public class User
    {
        public string name;
        public int age;
        public string status;

        public  User(string name, int age, string status)
        {
            this.name = name;
            this.age = age;
            this.status = status;
        }
    }

    public IEnumerator LoadData (string str)
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
                .ContinueWith(task2 => {
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
}
