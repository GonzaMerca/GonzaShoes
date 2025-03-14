﻿namespace GonzaShoes.Model.DTOs.User
{
    public class UserDTO : ContextualProps, ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
