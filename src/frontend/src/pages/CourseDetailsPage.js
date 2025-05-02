import { useParams } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { getCourseDetails } from '../api/api';
import AssignmentCard from '../components/AssignmentItem';

function CourseDetailsPage() {
    const { courseId } = useParams();
    const [course, setCourse] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const loadCourse = async () => {
            try {
                const data = await getCourseDetails(courseId);
                setCourse(data);
            } catch (error) {
                console.error('Failed to load course:', error);
            } finally {
                setLoading(false);
            }
        };
        loadCourse();
    }, [courseId]);

    if (loading) return <div className="loading">Loading course details...</div>;
    if (!course) return <div className="error">Course not found</div>;

    return (
        <div className="course-details">
            <h2>{course.name}</h2>
            <div className="course-links">
                <a href={course.googleLink} target="_blank" rel="noopener noreferrer">
                    Course Google Sheet
                </a>
                <a href={course.githubLink} target="_blank" rel="noopener noreferrer">
                    GitHub Organization
                </a>
            </div>

            <h3>Assignments</h3>
            <div className="assignments-list">
                {course.assignments.map(assignment => (
                    <AssignmentCard key={assignment.id} assignment={assignment} />
                ))}
            </div>
        </div>
    );
}

export default CourseDetailsPage;