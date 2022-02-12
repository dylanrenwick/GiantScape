using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GiantScape.Client.UI
{
    public class LoginButton : MonoBehaviour
    {
        [SerializeField]
        private int minUsernameLength;
        [SerializeField]
        private int minPasswordLength;

        [SerializeField]
        private Color idleColor;

        [SerializeField]
        private Color invalidColor;

        [SerializeField]
        private Color validColor;

        [SerializeField]
        private Text buttonText;

        [SerializeField]
        private InputField usernameField;

        [SerializeField]
        private InputField passwordField;

        [SerializeField]
        private Text errorText;

        private void Start()
        {
            ResetColor();
            errorText.enabled = false;
        }

        public void OnMouseEnter(BaseEventData eventData)
        {
            bool valid = Validate();
            SetColor(valid ? validColor : invalidColor);
            if (!valid)
            {
                GenerateErrors();
                errorText.enabled = true;
            }
        }

        public void OnMouseExit(BaseEventData eventData)
        {
            ResetColor();
            errorText.enabled = false;
        }

        private void ResetColor()
        {
            SetColor(idleColor);
        }
        private void SetColor(Color color)
        {
            buttonText.color = color;
        }

        private void GenerateErrors()
        {
            string username = usernameField.text;
            string password = passwordField.text;
            var sb = new StringBuilder();

            if (string.IsNullOrEmpty(username)) sb.AppendLine("No username provided");
            else if (username.Length < minUsernameLength) sb.AppendLine($"Username must be at least {minUsernameLength} characters");

            if (string.IsNullOrEmpty(password)) sb.AppendLine("No password provided");
            else if (password.Length < minPasswordLength) sb.AppendLine($"Password must be at least {minPasswordLength} characters");

            if (sb.Length == 0)
            {
                sb.AppendLine("Unknown error :(");
                sb.AppendLine("Please report this error at");
                sb.AppendLine("https://github.com/dylanrenwick/GiantScape/issues");
            }

            errorText.text = sb.ToString();
        }

        private bool Validate()
        {
            string username = usernameField.text;
            string password = passwordField.text;

            return (!string.IsNullOrEmpty(username)
                && !string.IsNullOrEmpty(password)
                && username.Length >= minUsernameLength
                && password.Length >= minPasswordLength);
        }
    }
}
