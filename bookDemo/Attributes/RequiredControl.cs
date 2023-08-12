class RequiredControl : Attribute{

    public static bool Dogrula(object dogrulanacakOrnek)
        {
            Type dogrulanacaktur = dogrulanacakOrnek.GetType();
            FieldInfo[] dogrulanacakturalanları = dogrulanacaktur.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo dogrulanacakturalani in dogrulanacakturalanları)
            {
                object[] zorunluAlanOznitelikleri = dogrulanacakturalani.GetCustomAttributes(typeof(ZorunluAlanAttribute), true);
                if (zorunluAlanOznitelikleri.Length != 0)
                {
                    string alanDegeri = dogrulanacakturalanları.GetValue((int)dogrulanacakOrnek) as string;
                    if (string.IsNullOrEmpty(alanDegeri))
                    {
                        return false;
                    }
                  
                }
              

            }
            return true;

        }
}