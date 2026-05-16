export interface User {
  id: number;
  username: string;
  email: string;
}

export interface Task {
  id: number;
  title: string;
  description: string;
  status: 'Nowe' | 'W trakcie' | 'Wykonane';
  priority: 'Niski' | 'Średni' | 'Wysoki';
  category: string;
  dueDate?: string;
  createdAt: string;
  userId: number;
}