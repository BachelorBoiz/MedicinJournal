import { Journal } from "./journal"

export interface Patient {
    id: number
    name: string
    birthDate: Date
    gender: string
    height: number
    weight: number
    journals: Journal[]
}
