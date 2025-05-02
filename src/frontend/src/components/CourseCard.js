import { Link } from 'react-router-dom';
import './CourseCard.css';

function CourseCard({ course }) {
    return (
        <div className="course-card">
            <h3 className="course-title">{course.name}</h3>
            <p className="course-description">{course.description || 'No description available'}</p>
            <Link to={`/courses/${course.id}`} className="view-btn">
                View Course
            </Link>
        </div>
    );
}

export default CourseCard;
