using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentBusinessLayer;
using StudentDataAccessLayer;

namespace StudentFullPrject.Controllers
{
    [Route("api/StudentApi")]
    [ApiController]
    public class StudentApiContollers : ControllerBase
    {
        [HttpGet("All",Name ="GetAllStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StudentDTO>> GetAllStudent()
        {
            List<StudentDTO> StudentsList = StudentBusinessLayer.Student.GetAllStuddent();
            if(StudentsList.Count==0)
            {
                return NotFound("Not Student Found!");
            }
            return Ok(StudentsList);
        }

        [HttpGet("Pass", Name = "GetPassStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StudentDTO>> GetPassStudent()
        {
            List<StudentDTO> StudentsList = StudentBusinessLayer.Student.GetPassStudent();
            if (StudentsList.Count == 0)
            {
                return NotFound("Not Student Found!");
            }
            return Ok(StudentsList);
        }

        [HttpGet("Average", Name = "GetAverageGrade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StudentDTO>> GetAverageGrade()
        {
            double Average=StudentBusinessLayer.Student.GetAverageGrade();
            return Ok(Average);
        }

        [HttpGet("{id}", Name = "GetStudentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<StudentDTO>> GetStudentByID(int id)
        {
            if(id<1)
            {
                return BadRequest($"Not Accepted ID= {id}");
            }

            StudentBusinessLayer.Student student = StudentBusinessLayer.Student.Find(id);
            if(student == null)
            {
                return NotFound($"Student with ID = {id} Not Found.");
            }
            StudentDTO SDTO=student.SDTO;
            return Ok(SDTO);
        }

        [HttpPost(Name = "AddStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<StudentDTO> AddStudent(StudentDTO NewStudentDTO)
        {
            if(NewStudentDTO==null||string.IsNullOrEmpty(NewStudentDTO.Name)||NewStudentDTO.Age<0||NewStudentDTO.Grade<0)
            {
                return BadRequest("Invaled Student Data");


            }



            StudentBusinessLayer.Student student = new StudentBusinessLayer.Student(new StudentDTO(NewStudentDTO.Id, NewStudentDTO.Name, NewStudentDTO.Age, NewStudentDTO.Grade));
            student.Save();

            NewStudentDTO.Id = student.ID;

            return CreatedAtRoute("GetStudentByID", new { id = NewStudentDTO.Id }, NewStudentDTO);
        }

        [HttpPut(Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> UpdateStudent(int id,StudentDTO UpdateStudentDTO)
        {
            if (id<1|| UpdateStudentDTO == null || string.IsNullOrEmpty(UpdateStudentDTO.Name) || UpdateStudentDTO.Age < 0 || UpdateStudentDTO.Grade < 0)
            {
                return BadRequest("Invaled Student Data");
            }

            StudentBusinessLayer.Student student = StudentBusinessLayer.Student.Find(id);
            if (student == null)
            {
                return NotFound($"Student with ID {id} not Found.");
            }

            student.Name = UpdateStudentDTO.Name;
            student.Age = UpdateStudentDTO.Age; 
            student.Grade= UpdateStudentDTO.Grade;
            if (student.Save())
                return Ok(student.SDTO);
            else
                return StatusCode(500, new { Mesaage = "Error Update Student" });
        }

        [HttpDelete("{id}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteStudent(int id)
        {
            if(id<1)
            {
                return BadRequest("Invaled Student Data");
            }

            if (StudentBusinessLayer.Student.DeleteStudent(id))
                return Ok($"Student with ID {id} Deleted");
            else
                return NotFound($"Student with ID {id} Not Found");
        }

    }
}
