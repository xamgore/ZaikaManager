using System;

namespace ZaikaManager.DB.Attributes {
    public abstract class FakerAttribute : Attribute {
        public abstract object Generate();
    }
}