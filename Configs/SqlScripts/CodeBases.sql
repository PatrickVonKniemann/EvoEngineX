-- Truncate the table before inserting new data
TRUNCATE TABLE public."CodeBases";

INSERT INTO public."CodeBases" ("Id", "Code", "UserId", "Name", "SupportedPlatform", "Valid", "CreatedAt")
VALUES ('123e4567-e89b-12d3-a456-426614174006','import java.util.Random;public class DifferentialEvolution {private static final int POP_SIZE = 50;private static final int DIMENSIONS = 10;private static final int MAX_ITER = 1000;private static final double F = 0.8;private static final double CR = 0.9;private static Random rand = new Random();public static void main(String[] args) {double[][] population = initializePopulation(POP_SIZE, DIMENSIONS);double[] bestSolution = evolve(population);System.out.println("Best solution found: ");for (double v : bestSolution) {System.out.print(v + " ");}System.out.println("\nFitness: " + evaluate(bestSolution));}private static double[][] initializePopulation(int popSize, int dimensions) {double[][] population = new double[popSize][dimensions];for (int i = 0; i < popSize; i++) {for (int j = 0; j < dimensions; j++) {population[i][j] = rand.nextDouble();}}return population;}private static double[] evolve(double[][] population) {double[] best = population[0];for (int iter = 0; iter < MAX_ITER; iter++) {for (int i = 0; i < POP_SIZE; i++) {double[] target = population[i];double[] trial = mutateAndCrossover(population, i);if (evaluate(trial) < evaluate(target)) {population[i] = trial;if (evaluate(trial) < evaluate(best)) {best = trial;}}}}return best;}private static double[] mutateAndCrossover(double[][] population, int targetIndex) {int r1, r2, r3;do { r1 = rand.nextInt(POP_SIZE); } while (r1 == targetIndex);do { r2 = rand.nextInt(POP_SIZE); } while (r2 == targetIndex || r2 == r1);do { r3 = rand.nextInt(POP_SIZE); } while (r3 == targetIndex || r3 == r1 || r3 == r2);double[] mutant = new double[DIMENSIONS];for (int j = 0; j < DIMENSIONS; j++) {mutant[j] = population[r1][j] + F * (population[r2][j] - population[r3][j]);}double[] trial = new double[DIMENSIONS];for (int j = 0; j < DIMENSIONS; j++) {if (rand.nextDouble() < CR) {trial[j] = mutant[j];} else {trial[j] = population[targetIndex][j];}}return trial;}private static double evaluate(double[] solution) {double sum = 0;for (double v : solution) {sum += v * v;}return sum;}}',
        '123e4567-e89b-12d3-a456-426614174008', 'Java DE algoritm', 1, true, NOW() - INTERVAL '1 day'),
       ('123e4567-e89b-12d3-a456-426614174008', 'D = [10, 30, 50, 100]; runs = 25; fistar = 100:100:3000; fistar = fistar(1:30); for j = 1:length(D), a = -100 + zeros(D(j), 1); b = 100 + zeros(D(j), 1); maxFES = 10000 * D(j); for i = [1, 3:30], func_num = i; fid = fopen(''jSO_cec24.txt'', ''a''); res = zeros(1000, runs); for r = 1:runs, [fmin_stage, FES, succ] = jSO_cec24(D(j), maxFES, a'', b'', func_num, fistar(func_num)); res(:, r) = fmin_stage; display(FES); display(''function ''); i; display('' run ''); r; display(fmin_stage(end)); fprintf(fid, ''%-10s %4.0f %4.0f %4.0f %9.0f %5.0f %16.6g %1s\n'', ''jSO'', func_num, D(j), r, FES, succ, fmin_stage(end), '' ''); end, fclose(fid); end, end',
        '123e4567-e89b-12d3-a456-426614174008', 'MatlabJSO', 2, true, NOW() - INTERVAL '3 days');
