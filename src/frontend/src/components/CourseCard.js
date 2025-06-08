import { Link } from 'react-router-dom';
import '../App.css';

function CourseCard({ course }) {
    return (
        <div className="course-card">
            <h4 className="subject-title">{course.subjectTitle}</h4>
            <h3 className="course-title">{course.title}</h3>
            <Link to={`/courses/${course.id}`} className="view-btn">
                Перейти к курсу
            </Link>
        </div>
    );
}

export default CourseCard;