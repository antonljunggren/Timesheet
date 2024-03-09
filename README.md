# Timesheet

This is a timesheet app for myself to keep track on my time when working on side projects.

The core premise is each calendar day has zero or many "TimeWorked" and each ItemWorked is connected to a Project.

### Domain
- Project
	- Id
	- Name

- TimeWorked
	- Id
	- ProjectId
	- Date
	- HoursWorked
	- Notes


### Future

Except for just using it myself, the idea is to grow the project step by step and turn it into a
multi tenant service.
And along the way document each step.
But that remains to be seen.