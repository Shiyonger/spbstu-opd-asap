import { format } from 'date-fns'; // Установите date-fns: npm install date-fns

function AssignmentItem({ assignment }) {
    return (
        <div className="assignment-card">
            <h4>{assignment.title}</h4>
            <p>{assignment.description}</p>
            <div className="assignment-meta">
                <span>Deadline: {format(new Date(assignment.deadline), 'dd.MM.yyyy HH:mm')}</span>
                <div className="links-container">
                    {assignment.googleSheetLink && (
                        <a href={assignment.googleSheetLink} target="_blank" rel="noopener noreferrer">
                            Google Sheets
                        </a>
                    )}
                    {assignment.githubLink && (
                        <a href={assignment.githubLink} target="_blank" rel="noopener noreferrer">
                            GitHub
                        </a>
                    )}
                </div>
            </div>
        </div>
    );
}

export default AssignmentItem;