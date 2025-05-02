import { Link } from 'react-router-dom';

function CourseCard({ course }) {
    return (
        <div className="course-card">
            <h3>{course.name}</h3>
            <p>{course.description || 'No description available'}</p>
            <Link to={`/courses/${course.id}`} className="view-btn">
                View Course
            </Link>
        </div>
    );
}

export default CourseCard;