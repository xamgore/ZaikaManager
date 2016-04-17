namespace ZaikaManager.DB.Attributes {
    public class CompanyAttribute : FakerAttribute {
        public override object Generate() => Faker.Company.Name();
    }
}