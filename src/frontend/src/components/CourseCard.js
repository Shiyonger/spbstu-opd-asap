import { Link } from 'react-router-dom';

function CourseCard({ course }) {
    return (
        <div className="course-card">
            <h3 className="course-title">{course.name}</h3>
            <p className="course-description">{course.description || 'No description available'}</p>
            <Link to={`/courses/${course.id}`} className="view-btn">
                Перейти к курсу
            </Link>
        </div>
    );
}

export default CourseCard;
