function AssignmentItem({ assignment }) {
    return (
        <div className="assignment-card">
            <h4>{assignment.title}</h4>
            <p>{assignment.description}</p>
            <div className="assignment-meta">
                <span>Points: {assignment.points}</span>
                <div className="links-container">
                    {assignment.githubLink && (
                        <a href={assignment.githubLink} target="_blank" rel="noopener noreferrer">
                            GitHub
                        </a>
                    )}
                    {assignment.googleSheetLink && (
                        <a href={assignment.googleSheetLink} target="_blank" rel="noopener noreferrer">
                            Google Sheets
                        </a>
                    )}
                </div>
            </div>
        </div>
    );
}

export default AssignmentItem;