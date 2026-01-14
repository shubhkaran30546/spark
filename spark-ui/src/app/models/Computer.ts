export interface Component {
  id: number;
  name: string;
  price: number;
  type: string;
}
export interface Computer {
  id: number;
  name: string;
  price: number;
  description: string;
  imageUrl: string;
  components?: Component[];
}
