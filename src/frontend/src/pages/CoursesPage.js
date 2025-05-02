import { useEffect, useState } from 'react';
import { getCourses } from '../api/api';
import CourseCard from '../components/CourseCard';

function CoursesPage() {
    const [courses, setCourses] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const loadCourses = async () => {
            try {
                const data = await getCourses();
                setCourses(data);
            } catch (error) {
                console.error('Failed to load courses:', error);
            } finally {
                setLoading(false);
            }
        };
        loadCourses();
    }, []);

    if (loading) return <div className="loading">Loading courses...</div>;

    return (
        <div className="courses-page">
            <h2>Available Courses</h2>
            <div className="courses-grid">
                {courses.map(course => (
                    <CourseCard key={course.id} course={course} />
                ))}
            </div>
        </div>
    );
}

export default CoursesPage;