using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Spark.Facade.Models;

namespace Spark.Facade.Extensions
{
    public static class SqlConnectionExtensions
    {
        public static SqlCommand CreateInsertCommandFrom(this SqlConnection connection, PatientModel patientModel)
        {
            var command = connection.CreateCommand();
            var patientModelType = patientModel.GetType();
            var propertyInfos = patientModelType.GetProperties();
            command.CommandText = GenerateInsertCommandText("Patient", propertyInfos);

            foreach (var propertyInfo in propertyInfos)
            {
                object value = propertyInfo.GetValue(patientModel);
                if (propertyInfo.PropertyType.IsArray && value != null)
                {
                    value = string.Join(' ', value);
                }

                command.Parameters.Add(new SqlParameter(propertyInfo.Name, value ?? DBNull.Value));
            }

            return command;
        }

        private static string GenerateInsertCommandText(string tablename, IEnumerable<PropertyInfo> propertyInfos)
        {
            var commandPart = $"INSERT INTO {tablename}(";
            var valuePart = "VALUES(";
            foreach (var propertyInfo in propertyInfos)
            {
                commandPart += $"{propertyInfo.Name},";
                valuePart += $"@{propertyInfo.Name},";
            }

            return $"{commandPart.TrimEnd(',')}){valuePart.TrimEnd(',')})";
        }

        public static SqlCommand CreateUpdateCommandFrom(this SqlConnection connection, PatientModel patientModel, string primaryKeyName, object primaryKeyValue)
        {
            var command = connection.CreateCommand();
            var patientModelType = patientModel.GetType();
            // TODO: Hack to remove primary key from update command
            var propertyInfos = patientModelType.GetProperties().Where(prop => prop.Name != primaryKeyName).ToArray();
            command.CommandText = $"{GenerateUpdateCommandText("Patient", propertyInfos)} WHERE {primaryKeyName}=@{primaryKeyName}";
            command.Parameters.Add(new SqlParameter(primaryKeyName, primaryKeyValue));
            foreach (var propertyInfo in propertyInfos)
            {
                object value = propertyInfo.GetValue(patientModel);
                if (propertyInfo.PropertyType.IsArray && value != null)
                {
                    value = string.Join(' ', value);
                }

                command.Parameters.Add(new SqlParameter(propertyInfo.Name, value ?? DBNull.Value));
            }

            return command;
        }

        private static string GenerateUpdateCommandText(string tablename, IEnumerable<PropertyInfo> propertyInfos)
        {
            var commandPart = $"UPDATE {tablename} SET";
            var valuePart = "";
            foreach (var propertyInfo in propertyInfos)
            {
                valuePart += $"{propertyInfo.Name}=@{propertyInfo.Name},";
            }

            return $"{commandPart} {valuePart.TrimEnd(',')}";
        }
        
        public static SqlCommand CreateSelectCommandByPrimaryKeyFrom(this SqlConnection connection, string tablename, string primaryKeyName, object primaryKeyValue, Type patientModelType)
        {
            var propertyInfos = patientModelType.GetProperties();
            var commandText = GenerateSelectCommandText(tablename, propertyInfos);
            
            var command = connection.CreateCommand();
            command.CommandText = $"{commandText} WHERE {primaryKeyName}=@{primaryKeyName}";
            command.Parameters.Add(new SqlParameter(primaryKeyName, primaryKeyValue));
            
            return command;
        }

        public static SqlCommand CreateSelectCommandWithCriteriaFrom(this SqlConnection connection, string tablename, IDictionary<string, object> crtierias, Type patientModelType)
        {
            var propertyInfos = patientModelType.GetProperties();
            var commandText = GenerateSelectCommandText(tablename, propertyInfos);

            var command = connection.CreateCommand();
            // TODO: Currently just ignoring any other parameters beyond the first one 
            var criteria = crtierias.FirstOrDefault();
            command.CommandText = $"{commandText} WHERE {criteria.Key}=@{criteria.Key}";
            command.Parameters.Add(new SqlParameter(criteria.Key, criteria.Value));

            return command;
        }

        private static string GenerateSelectCommandText(string tableName, IEnumerable<PropertyInfo> propertyInfos)
        {
            var parameters = "";
            foreach (var propertyInfo in propertyInfos)
            {
                parameters += $"{propertyInfo.Name},";
            }

            return $"SELECT {parameters.TrimEnd(',')} FROM {tableName}";
        }

        public static SqlCommand CreateExistsCommandByPrimaryKeyFrom(this SqlConnection connection, string tablename, string primaryKeyName, object primaryKeyValue)
        {
            var command = connection.CreateCommand();
            command.CommandText = $"SELECT COUNT(*) FROM {tablename} WHERE {primaryKeyName}=@{primaryKeyName}";
            command.Parameters.Add(new SqlParameter(primaryKeyName, primaryKeyValue));
            return command;
        }
    }
}
