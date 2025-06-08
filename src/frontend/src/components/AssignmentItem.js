import { format } from 'date-fns'; // Установите date-fns: npm install date-fns
import '../App.css';

function AssignmentItem({ assignment }) {
    return (
        <div className="assignment-card">
            <h4>{assignment.title}</h4>
            <p>{assignment.description}</p>
            <p>Максимум баллов: {assignment.maxPoints}</p>
            <div className="assignment-meta">
                <span className="assignment-deadline">
                    Deadline: {format(new Date(assignment.dueTo), 'dd.MM.yyyy HH:mm')}
                </span>
                <div className="links-container">
                    {assignment.link && (
                        <a href={assignment.link} target="_blank" rel="noopener noreferrer">
                            Перейти к заданию
                        </a>
                    )}
                </div>
            </div>
        </div>
    );
}

export default AssignmentItem;