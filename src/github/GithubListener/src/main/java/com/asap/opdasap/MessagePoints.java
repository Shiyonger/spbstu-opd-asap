package com.asap.opdasap;

import com.fasterxml.jackson.annotation.JsonProperty;

import java.io.Serializable;
import java.util.Date;

public final class MessagePoints implements Serializable {

    @JsonProperty("assignment_name")
    private String assignmentName;

    @JsonProperty("course_title")
    private String courseTitle; // ← новое поле

    @JsonProperty("date")
    private Date date;

    @JsonProperty("points")
    private int points;

    @JsonProperty("student_position")
    private Position studentPosition;

    @JsonProperty("assignment_position")
    private Position assignmentPosition;

    public String getAssignmentName() {
        return assignmentName;
    }

    public void setAssignmentName(String assignmentName) {
        this.assignmentName = assignmentName;
    }

    public String getCourseTitle() {
        return courseTitle;
    }

    public void setCourseTitle(String courseTitle) {
        this.courseTitle = courseTitle;
    }

    public Date getDate() {
        return date;
    }

    public void setDate(Date date) {
        this.date = date;
    }

    public int getPoints() {
        return points;
    }

    public void setPoints(int points) {
        this.points = points;
    }

    public Position getStudentPosition() {
        return studentPosition;
    }

    public void setStudentPosition(Position studentPosition) {
        this.studentPosition = studentPosition;
    }

    public Position getAssignmentPosition() {
        return assignmentPosition;
    }

    public void setAssignmentPosition(Position assignmentPosition) {
        this.assignmentPosition = assignmentPosition;
    }

    public static final class Position {
        @JsonProperty("cell")
        private String cell;

        @JsonProperty("spreadsheet_id")
        private String spreadsheetId;

        public String getCell() {
            return cell;
        }

        public void setCell(String cell) {
            this.cell = cell;
        }

        public String getSpreadsheetId() {
            return spreadsheetId;
        }

        public void setSpreadsheetId(String spreadsheetId) {
            this.spreadsheetId = spreadsheetId;
        }

        // ... геттеры/сеттеры
    }
}
