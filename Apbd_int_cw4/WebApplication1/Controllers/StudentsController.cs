using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Apbd_int_cw4.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase

    {
         
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s17174;Integrated Security=True";
        /*
        [HttpGet]
        public IActionResult GetStudentsTest()
        {
            //1. Nisko-poziomowa
            //  1.2. Sterowniki Oracle/ SqlDServer
            //  1.2 Sterowniki ogólne
            //DbConnection + DbCommand

            var list = new List<Student>();

            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {

                com.Connection = con;
                com.CommandText = "Select * from student";

                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.IdEnrollment = dr["IdEnrollment"].ToString();
                    list.Add(st);
                }
            }
            return Ok(list);
        }
        */
        /*
        [HttpGet("{indexNumber}")]
        public IActionResult GetStudentWithIndex(string indexNumber)
        {
            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {

                com.Connection = con;
                com.CommandText = "Select * from student where indexNumber=@index";

                com.Parameters.AddWithValue("index", indexNumber);
                con.Open();
                var dr = com.ExecuteReader();
                if (dr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.IdEnrollment = dr["IdEnrollment"].ToString();
                    return Ok(st);
                }

            }
            return NotFound();
        }

        */
     
        //Zadanie 4.2.3 ++
        [HttpGet]
        public IActionResult GetStudent()
        {
            var students = new List<StudentInfo>();
            using (var client = new SqlConnection(ConString))
            using (var command = new SqlCommand())
            {
                command.Connection = client;
                command.CommandText = "SELECT s.FirstName, s.LastName, s.BirthDate, st.Name, e.Semester " +
                    "FROM Student s " +
                    "INNER JOIN Enrollment e " +
                    "ON s.IdEnrollment = e.IdEnrollment " +
                    "INNER JOIN Studies st " +
                    "ON st.IdStudy = e.IdStudy;";

                client.Open();
                var dr = command.ExecuteReader();


                while (dr.Read())
                {
                    students.Add(new StudentInfo
                    {
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        BirthDate = Convert.ToDateTime(dr["BirthDate"].ToString()),
                        Name = dr["Name"].ToString(),
                        Semester = int.Parse(dr["Semester"].ToString())
                    });

                }
            }

            return Ok(students);
        }

        [HttpGet("{index}")]
        public IActionResult GetStudent(String index)
        {
            List<Enrollment> enrollments = new List<Enrollment>();
            using (var connection = new SqlConnection(ConString))
            using (var commands = new SqlCommand())
            {
                commands.Connection = connection;

                commands.CommandText = $"SELECT * FROM Enrollment e " +
                    $"INNER JOIN Student s " +
                    $"ON e.IdEnrollment = s.IdEnrollment " +
                    $"WHERE s.IndexNumber = @index;";

                commands.Parameters.AddWithValue("index", index);   //zabezpieczenie przed sql-injection


                connection.Open();
                var dr = commands.ExecuteReader();

                while (dr.Read())

                {
                    var enrollment = new Enrollment();
                    enrollment.IdEnrollment = Int32.Parse(dr["IdEnrollment"].ToString());
                    enrollment.Semester = Int32.Parse(dr["Semester"].ToString());
                    enrollment.IdStudy = Int32.Parse(dr["IdStudy"].ToString());
                    enrollment.StartDate = dr["StartDate"].ToString();

                    enrollments.Add(enrollment);
                }

            }


            return Ok(enrollments);
        }


        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }
    }
}
