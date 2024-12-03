export function isValid(input: string | null, maxLength: number): boolean {
  return input != null && input.length > 0 && input.length <= maxLength;
}
