import { useParams } from 'react-router-dom';
import { useEffect, useState } from 'react';
import {getCourseDetails, getCourses} from '../api/api';
import AssignmentItem from '../components/AssignmentItem';
import HeaderButtons from '../components/HeaderButtons';
import './CourseDetailsPage.css';
import '../App.css';

function CourseDetailsPage() {
    const { courseId } = useParams();
    const [assignments, setAssignments] = useState(null);
    const [courseTitle, setCourseTitle] = useState(null);
    const [githubLink, setGithubLink] = useState(null);
    const [googleLink, setGoogleLink] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const loadCourse = async () => {
            try {
                const data = await getCourseDetails(courseId);
                const courseData = await getCourses();
                for (let i = 0; i < courseData.length; i++) {
                    if (courseData[i].id == courseId) {
                        setGithubLink(courseData[i].githubOrganizationLink);
                        setGoogleLink(courseData[i].googleSpreadsheetLink);
                        setCourseTitle(courseData[i].title)
                    }
                }
                setAssignments(data);
            } catch (error) {
                console.error('Не удалось загрузить курс', error);
            } finally {
                setLoading(false);
            }
        };
        loadCourse();
    }, [courseId]);

    if (loading) return <div className="course-loading">Загрузка информации о курсе...</div>;
    if (!assignments) return <div className="course-error">Курс не найден</div>;

    return (
        <div className="course-details-background">
            <HeaderButtons />
            <h2 className="course-title1">{courseTitle}</h2>
            <div className="course-links">
                <a href={googleLink} target="_blank" rel="noopener noreferrer">
                    Google Sheets
                </a>
                <a href={githubLink} target="_blank" rel="noopener noreferrer">
                    GitHub
                </a>
            </div>

            <h3 className="assignments-title">Задания</h3>
            <div className="assignments-list">
                {assignments.map((assignment) => (
                    <AssignmentItem key={assignment.id} assignment={assignment} />
                ))}
            </div>
        </div>
    );
}

export default CourseDetailsPage;
