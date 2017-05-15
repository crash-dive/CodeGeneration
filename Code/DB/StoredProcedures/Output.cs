namespace FleetAssist.Data.SP.Schema
{
    public class Address
    {
        public IList<RapidAddress_Result> RapidAddress(string @postcode)
        {
            return RapidAddress(@postcode, new List<RapidAddress_Result>(), r => r);
        }

        public IList<TReturn> RapidAddress<TReturn>(string @postcode, Func<RapidAddress_Result, TReturn> resultToList)
        {
            return RapidAddress(@postcode, new List<TReturn>(), resultToList);
        }

        public IList<TReturn> RapidAddress<TReturn>(string @postcode, IList<TReturn> list, Func<RapidAddress_Result, TReturn> resultToList)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["AtlasStoredProc"].ConnectionString))
            using (var cmd = new SqlCommand("Address.RapidAddress", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Param("@Postcode", SqlDbType.NVarChar, 8, @postcode);

                con.Open();
                return cmd.ReadToList(list, r => new RapidAddress_Result(r), resultToList);
            }
        }

        public class RapidAddress_Result
        {
            public string Organisation { get; }
            public string SubBuildingName { get; }
            public string BuildingName { get; }
            public string BuildingNumber { get; }
            public string Thoroughfare { get; }
            public string DependentThoroughfare { get; }
            public string DoubleDependentLocality { get; }
            public string DependentLocality { get; }
            public string PostTown { get; }

            internal RapidAddress_Result(SqlDataReader reader)
            {
                if (reader.IsDBNull(0) == false) { Organisation = reader.GetString(0); }
                if (reader.IsDBNull(1) == false) { SubBuildingName = reader.GetString(1); }
                if (reader.IsDBNull(2) == false) { BuildingName = reader.GetString(2); }
                if (reader.IsDBNull(3) == false) { BuildingNumber = reader.GetString(3); }
                if (reader.IsDBNull(4) == false) { Thoroughfare = reader.GetString(4); }
                if (reader.IsDBNull(5) == false) { DependentThoroughfare = reader.GetString(5); }
                if (reader.IsDBNull(6) == false) { DoubleDependentLocality = reader.GetString(6); }
                if (reader.IsDBNull(7) == false) { DependentLocality = reader.GetString(7); }
                PostTown = reader.GetString(8);
            }
        }

        public IList<SearchPostcodeLocation_Result> SearchPostcodeLocation(string @postcode, bool? @createdManually, int? @pageOffset, int? @pageSize, string @sortDirection, string @sortPropertyName)
        {
            return SearchPostcodeLocation(@postcode, @createdManually, @pageOffset, @pageSize, @sortDirection, @sortPropertyName, new List<SearchPostcodeLocation_Result>(), r => r);
        }

        public IList<TReturn> SearchPostcodeLocation<TReturn>(string @postcode, bool? @createdManually, int? @pageOffset, int? @pageSize, string @sortDirection, string @sortPropertyName, Func<SearchPostcodeLocation_Result, TReturn> resultToList)
        {
            return SearchPostcodeLocation(@postcode, @createdManually, @pageOffset, @pageSize, @sortDirection, @sortPropertyName, new List<TReturn>(), resultToList);
        }

        public IList<TReturn> SearchPostcodeLocation<TReturn>(string @postcode, bool? @createdManually, int? @pageOffset, int? @pageSize, string @sortDirection, string @sortPropertyName, IList<TReturn> list, Func<SearchPostcodeLocation_Result, TReturn> resultToList)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["AtlasStoredProc"].ConnectionString))
            using (var cmd = new SqlCommand("Address.SearchPostcodeLocation", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Param("@Postcode", SqlDbType.NVarChar, 8, @postcode);
                cmd.Param("@CreatedManually", SqlDbType.Bit, 1, @createdManually);
                cmd.Param("@PageOffset", SqlDbType.Int, 4, @pageOffset);
                cmd.Param("@PageSize", SqlDbType.Int, 4, @pageSize);
                cmd.Param("@SortDirection", SqlDbType.NVarChar, 10, @sortDirection);
                cmd.Param("@SortPropertyName", SqlDbType.NVarChar, 100, @sortPropertyName);

                con.Open();
                return cmd.ReadToList(list, r => new SearchPostcodeLocation_Result(r), resultToList);
            }
        }

        public class SearchPostcodeLocation_Result
        {
            public string Postcode { get; }
            public double? Lat { get; }
            public double? Long { get; }
            public bool CreatedManually { get; }

            internal SearchPostcodeLocation_Result(SqlDataReader reader)
            {
                Postcode = reader.GetString(0);
                if (reader.IsDBNull(1) == false) { Lat = reader.GetDouble(1); }
                if (reader.IsDBNull(2) == false) { Long = reader.GetDouble(2); }
                CreatedManually = reader.GetBoolean(3);
            }
        }

    }

    public class AddressImport
    {
        public void ClearAddressesForPAFImport()
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["AtlasStoredProc"].ConnectionString))
            using (var cmd = new SqlCommand("AddressImport.ClearAddressesForPAFImport", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ImportPAFAddress()
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["AtlasStoredProc"].ConnectionString))
            using (var cmd = new SqlCommand("AddressImport.ImportPAFAddress", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ImportPAFOrganisation()
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["AtlasStoredProc"].ConnectionString))
            using (var cmd = new SqlCommand("AddressImport.ImportPAFOrganisation", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ImportPostcodeLocationStage()
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["AtlasStoredProc"].ConnectionString))
            using (var cmd = new SqlCommand("AddressImport.ImportPostcodeLocationStage", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

    }
}