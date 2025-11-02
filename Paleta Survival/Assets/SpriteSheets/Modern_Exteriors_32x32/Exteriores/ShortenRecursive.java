package BreakingCat_Unity_Repo.BreakingCat_Project.Assets.SpriteSheets.Modern_Exteriors_32x32.Modern_Exteriors_Complete_Singles_32x32;
import java.io.File;
import java.util.HashSet;
import java.util.Set;

public class ShortenRecursive {

    private static final int MAX_NAME_LENGTH = 20; // Máximo de caracteres para el nombre (sin extensión)
    private static final Set<String> usedNames = new HashSet<>();

    public static void main(String[] args) {
        File baseDir = new File(".");
        processDirectory(baseDir);
    }

    private static void processDirectory(File dir) {
        File[] entries = dir.listFiles();
        if (entries == null) return;

        for (File entry : entries) {
            if (entry.isDirectory()) {
                processDirectory(entry); // Recursivamente
            } else if (entry.isFile()) {
                shortenFileName(entry);
            }
        }
    }

    private static void shortenFileName(File file) {
        String fullName = file.getName();
        int dotIndex = fullName.lastIndexOf('.');
        String name = (dotIndex >= 0) ? fullName.substring(0, dotIndex) : fullName;
        String extension = (dotIndex >= 0) ? fullName.substring(dotIndex) : "";

        if (name.length() <= MAX_NAME_LENGTH) {
            return; // Ya es suficientemente corto
        }

        String shortenedName = name.substring(0, MAX_NAME_LENGTH);
        File parent = file.getParentFile();
        File newFile = new File(parent, shortenedName + extension);
        int count = 1;

        // Asegurar que no sobrescribimos otro archivo
        while (newFile.exists() || usedNames.contains(newFile.getAbsolutePath())) {
            String suffix = "_" + count;
            int trimLength = Math.max(0, MAX_NAME_LENGTH - suffix.length());
            shortenedName = name.substring(0, trimLength) + suffix;
            newFile = new File(parent, shortenedName + extension);
            count++;
        }

        if (file.renameTo(newFile)) {
            usedNames.add(newFile.getAbsolutePath());
            System.out.println("Renombrado: " + file.getAbsolutePath() + " -> " + newFile.getAbsolutePath());
        } else {
            System.out.println("Error al renombrar: " + file.getAbsolutePath());
        }
    }
}
