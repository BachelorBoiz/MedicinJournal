import { Patient } from "./patient"

export interface Employee {
    id: number
    name: string
    patients: Patient[]
}
